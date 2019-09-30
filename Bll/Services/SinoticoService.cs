using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class SinoticoService : Services<Sinotico> {
    private readonly int userId;

    public SinoticoService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Sinotico> Get(Expression<Func<Sinotico, bool>> filter = null,
        Func<IQueryable<Sinotico>, IOrderedQueryable<Sinotico>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<Sinotico> query = (from s in context.Sinoticos
                                      join l in context.Linhas on s.LinhaId equals l.Id
                                      where companies.Contains(l.EmpresaId)
                                      orderby l.EmpresaId, s.LinhaId, s.DiaId, s.Sentido, s.SinoticoId
                                      select s).AsNoTracking()
                                          .Include(s => s.Linha.Empresa).Include(s => s.ISinotico);
        if (filter != null) {
          query = query.Where(filter);
        }
        if (orderBy != null) {
          query = orderBy(query);
        }
        return query;
      }
      catch (Exception ex) {
        throw new Exception(ex.Message);
      }
    }

    public override IQueryable<Sinotico> GetQuery() {
      HashSet<Sinotico> result = new HashSet<Sinotico>();
      foreach (Sinotico item in Get()) {
        result.Add(ComputeIndices(item));
      }
      return result.AsQueryable();
    }

    public override IQueryable<Sinotico> GetQuery(Expression<Func<Sinotico, bool>> filter = null,
        Func<IQueryable<Sinotico>, IOrderedQueryable<Sinotico>> orderBy = null, int? skip = null, int? take = null) {
      HashSet<Sinotico> result = new HashSet<Sinotico>();
      foreach (Sinotico item in Get(filter, orderBy)) {
        result.Add(ComputeIndices(item));
      }
      return result.AsQueryable();
    }

    public override IEnumerable<Sinotico> GetAll() {
      return GetQuery().ToList();
    }

    public override IEnumerable<Sinotico> GetAll(Expression<Func<Sinotico, bool>> filter = null,
        Func<IQueryable<Sinotico>, IOrderedQueryable<Sinotico>> orderBy = null) {
      return GetQuery(filter, orderBy).ToList();
    }

    /*
     * Quadro Sinotico da Linha (Indice Atual, Prognostico Estatistico, 
     * Evolucao Estatistica, Prognostico Projeto, Evolucao Projeto).
     */
    protected Sinotico ComputeIndices(Sinotico current) {
      using (Services<Dimensionamento> dimensionamento = new Services<Dimensionamento>()) {
        Expression<Func<Dimensionamento, bool>> filter = d => (d.LinhaId == current.LinhaId) && (d.DiaId == current.DiaId) &&
                                                               d.Sentido.Equals(current.Sentido);
        IQueryable<Dimensionamento> query = dimensionamento.GetQuery(filter);

        int prognosticoE = 0;
        int prognosticoP = 0;
        int duracao = 0;
        foreach (Dimensionamento item in query) {
          prognosticoE += item.PrognosticoE ?? 0;
          prognosticoP += item.PrognosticoP ?? 0;
          duracao += item.Duracao;
        }

        CustoMod custos;
        using (Services<Linha> linhas = new Services<Linha>()) {
          int empresaId = linhas.GetById(current.LinhaId).EmpresaId;

          using (Services<CustoMod> custosMod = new Services<CustoMod>()) {
            custos = custosMod.GetFirst(q => q.EmpresaId == empresaId);
          }
        }

        switch (current.SinoticoId) {
          case 1:      // Volume de Passageiros (Pass/dia)
            current.IndiceAtual = string.Format("{0:#,##0}", query.Sum(q => q.Ajustado));
            current.DimensionaE = current.IndiceAtual;
            current.DimensionaP = current.IndiceAtual;
            break;
          case 2:      // Numero de Viagens (Unidirecionais) (Viagens/dia)
            current.IndiceAtual = string.Format("{0:#,##0}", query.Sum(q => q.QtdViagens));
            current.DimensionaE = string.Format("{0:#,##0}", prognosticoE);
            current.DimensionaP = string.Format("{0:#,##0}", prognosticoP);
            try {
              current.EvolucaoE = -(1 - ((decimal)prognosticoE / query.Sum(q => q.QtdViagens)));
              current.EvolucaoP = -(1 - ((decimal)prognosticoP / query.Sum(q => q.QtdViagens)));
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }            
            break;
          case 3:      // Extensao das Viagens (km)
            current.IndiceAtual = string.Format("{0:#,##0.0}", query.Max(q => q.Extensao));
            current.DimensionaE = current.IndiceAtual;
            current.DimensionaP = current.IndiceAtual;
            break;
          case 4:      // Quilometragem Percorrida (km/dia)
            current.IndiceAtual = string.Format("{0:#,##0.0}", query.Sum(q => q.QtdViagens) * query.Max(q => q.Extensao));
            current.DimensionaE = string.Format("{0:#,##0.0}", prognosticoE * query.Max(q => q.Extensao));
            current.DimensionaP = string.Format("{0:#,##0.0}", prognosticoP * query.Max(q => q.Extensao));
            try {
              current.EvolucaoE = -(1 - (prognosticoE * query.Max(q => q.Extensao) /
                                          (query.Sum(q => q.QtdViagens) * query.Max(q => q.Extensao))));
              current.EvolucaoP = -(1 - (prognosticoP * query.Max(q => q.Extensao) /
                                          (query.Sum(q => q.QtdViagens) * query.Max(q => q.Extensao))));
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }            
            break;
          case 5:      // Pontos de Parada
            int? total;
            using (Services<PtLinha> pontos = new Services<PtLinha>()) {
              total = pontos.GetCount(q => (q.LinhaId == current.LinhaId) && q.Sentido.Equals(current.Sentido));
            }
            current.IndiceAtual = string.Format("{0:#,###}", total);
            current.DimensionaE = current.IndiceAtual;
            current.DimensionaP = current.IndiceAtual;
            break;
          case 6:      // Tempo de Ciclo (Min.)
            current.IndiceAtual = string.Format("{0:#,##0}",
                                      GetTempoCiclo(d => (d.LinhaId == current.LinhaId) && (d.DiaId == current.DiaId)));
            current.DimensionaE = current.IndiceAtual;
            current.DimensionaP = current.IndiceAtual;
            break;
          case 7:      // Ociosidade (%)
            try {
              current.IndiceAtual = string.Format("{0:P2}", (decimal?)query.Sum(q => q.Ociosidade) / duracao);
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            break;
          case 8:      // Velocidade Comercial (Pico) (km/h)
            try {
              current.IndiceAtual = string.Format("{0:#,##0.0}", query.Sum(q => q.QtdViagens) * query.Max(q => q.Extensao) /
                                                                   duracao * 60 / GetPeriodoPico(filter).Veiculos);
              current.DimensionaE = string.Format("{0:#,##0.0}", prognosticoE * query.Max(q => q.Extensao) /
                                                                   duracao * 60 / GetPeriodoPico(filter).VeiculosE);
              current.DimensionaP = string.Format("{0:#,##0.0}", prognosticoP * query.Max(q => q.Extensao) /
                                                                   duracao * 60 / GetPeriodoPico(filter).VeiculosP);
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            try {
              current.EvolucaoE = -(1 - prognosticoE * query.Max(q => q.Extensao) /
                                          duracao * 60 / GetPeriodoPico(filter).VeiculosE /
                                        (query.Sum(q => q.QtdViagens) * query.Max(q => q.Extensao) /
                                          duracao * 60 / GetPeriodoPico(filter).Veiculos));
              current.EvolucaoP = -(1 - prognosticoP * query.Max(q => q.Extensao) /
                                          duracao * 60 / GetPeriodoPico(filter).VeiculosP /
                                        (query.Sum(q => q.QtdViagens) * query.Max(q => q.Extensao) /
                                          duracao * 60 / GetPeriodoPico(filter).Veiculos));
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            break;
          case 9:      // Velocidade Comercial (Media) (km/h)
            try {
              current.IndiceAtual = string.Format("{0:#,##0.0}", query.Sum(q => q.QtdViagens) * query.Max(q => q.Extensao) /
                                                                   duracao * 60 / GetMediaVeiculos(filter));
              current.DimensionaE = string.Format("{0:#,##0.0}", prognosticoE * query.Max(q => q.Extensao) /
                                                                   duracao * 60 / GetMediaVeiculosE(filter));
              current.DimensionaP = string.Format("{0:#,##0.0}", prognosticoP * query.Max(q => q.Extensao) /
                                                                   duracao * 60 / GetMediaVeiculosP(filter));
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            try {
              current.EvolucaoE = -(1 - prognosticoE * query.Max(q => q.Extensao) / duracao * 60 / GetMediaVeiculosE(filter) /
                                          (query.Sum(q => q.QtdViagens) * query.Max(q => q.Extensao) /
                                            duracao * 60 / GetMediaVeiculos(filter)));
              current.EvolucaoP = -(1 - prognosticoP * query.Max(q => q.Extensao) / duracao * 60 / GetMediaVeiculosP(filter) /
                                          (query.Sum(q => q.QtdViagens) * query.Max(q => q.Extensao) /
                                            duracao * 60 / GetMediaVeiculos(filter)));
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            break;
          case 10:     // Frota Total (Pico)
            current.IndiceAtual = string.Format("{0:#,##0}", GetPeriodoPico(filter).Veiculos);
            current.DimensionaE = string.Format("{0:#,##0}", GetPeriodoPico(filter).VeiculosE);
            current.DimensionaP = string.Format("{0:#,##0}", GetPeriodoPico(filter).VeiculosP);
            try {
              current.EvolucaoE = -(1 - (decimal)GetPeriodoPico(filter).VeiculosE / GetPeriodoPico(filter).Veiculos);
              current.EvolucaoP = -(1 - (decimal)GetPeriodoPico(filter).VeiculosP / GetPeriodoPico(filter).Veiculos);
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            break;
          case 11:     // Frota Total (Media)
            current.IndiceAtual = string.Format("{0:#,##0}", GetMediaVeiculos(filter));
            current.DimensionaE = string.Format("{0:#,##0}", GetMediaVeiculosE(filter));
            current.DimensionaP = string.Format("{0:#,##0}", GetMediaVeiculosP(filter));
            try {
              current.EvolucaoE = -(1 - (decimal)GetMediaVeiculosE(filter) / GetMediaVeiculos(filter));
              current.EvolucaoP = -(1 - (decimal)GetMediaVeiculosP(filter) / GetMediaVeiculos(filter));
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            break;
          case 12:     // Custo Operacional (R$)
            if (custos != null) {              
              decimal? custo = (custos.Fixo + custos.Variavel) * query.Max(q => q.Extensao);
              current.IndiceAtual = string.Format("{0:#,##0.00}", query.Sum(q => q.QtdViagens) * custo);
              current.DimensionaE = string.Format("{0:#,##0.00}", prognosticoE * custo);
              current.DimensionaP = string.Format("{0:#,##0.00}", prognosticoP * custo);
              try {
                current.EvolucaoE = -(1 - (prognosticoE * custo / (query.Sum(q => q.QtdViagens) * custo)));
                current.EvolucaoP = -(1 - (prognosticoP * custo / (query.Sum(q => q.QtdViagens) * custo)));
              }
              catch (DivideByZeroException ex) {
                throw new Exception(ex.Message);
              }
            }
            break;
          case 13:     // Taxa de Utilizacao (IPK) (Pass/km)
            try {
              current.IndiceAtual = string.Format("{0:#,##0.000}", query.Sum(q => q.Ajustado) /
                                                                     (query.Sum(q => q.QtdViagens) * query.Max(q => q.Extensao)));
              current.DimensionaE = string.Format("{0:#,##0.000}", query.Sum(q => q.Ajustado) /
                                                                     (prognosticoE * query.Max(q => q.Extensao)));
              current.DimensionaP = string.Format("{0:#,##0.000}", query.Sum(q => q.Ajustado) /
                                                                     (prognosticoP * query.Max(q => q.Extensao)));
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            try {
              current.EvolucaoE = -(1 - query.Sum(q => q.Ajustado) / (prognosticoE * query.Max(q => q.Extensao)) /
                                          (query.Sum(q => q.Ajustado) /
                                            (query.Sum(q => q.QtdViagens) * query.Max(q => q.Extensao))));
              current.EvolucaoP = -(1 - query.Sum(q => q.Ajustado) / (prognosticoP * query.Max(q => q.Extensao)) /
                                          (query.Sum(q => q.Ajustado) /
                                            (query.Sum(q => q.QtdViagens) * query.Max(q => q.Extensao))));
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            break;
          case 14:     // Uso da Frota	(km/veic)
            try {
              current.IndiceAtual = string.Format("{0:#,##0.0}", query.Sum(q => q.QtdViagens) * query.Max(q => q.Extensao) /
                                                                   GetMediaVeiculos(filter));
              current.DimensionaE = string.Format("{0:#,##0.0}", prognosticoE * query.Max(q => q.Extensao) /
                                                                   GetMediaVeiculosE(filter));
              current.DimensionaP = string.Format("{0:#,##0.0}", prognosticoP * query.Max(q => q.Extensao) /
                                                                   GetMediaVeiculosP(filter));
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            try {
              current.EvolucaoE = -(1 - prognosticoE * query.Max(q => q.Extensao) / GetMediaVeiculosE(filter) /
                                          (query.Sum(q => q.QtdViagens) * query.Max(q => q.Extensao) /
                                            GetMediaVeiculos(filter)));
              current.EvolucaoP = -(1 - prognosticoP * query.Max(q => q.Extensao) / GetMediaVeiculosP(filter) /
                                          (query.Sum(q => q.QtdViagens) * query.Max(q => q.Extensao) /
                                            GetMediaVeiculos(filter)));
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            break;
          case 15:     // Rendimento da Frota (Pass/veic)
            try {
              current.IndiceAtual = string.Format("{0:#,##0}", query.Sum(q => q.Ajustado) / GetMediaVeiculos(filter));
              current.DimensionaE = string.Format("{0:#,##0}", query.Sum(q => q.Ajustado) / GetMediaVeiculosE(filter));
              current.DimensionaP = string.Format("{0:#,##0}", query.Sum(q => q.Ajustado) / GetMediaVeiculosP(filter));
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            try {
              current.EvolucaoE = -(1 - (decimal)query.Sum(q => q.Ajustado) / GetMediaVeiculosE(filter) /
                                                   (query.Sum(q => q.Ajustado) / GetMediaVeiculos(filter)));
              current.EvolucaoP = -(1 - (decimal)query.Sum(q => q.Ajustado) / GetMediaVeiculosP(filter) /
                                                   (query.Sum(q => q.Ajustado) / GetMediaVeiculos(filter)));
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            break;
          case 16:     // Custo do Transporte (R$/pass)
            if (custos != null) {
              decimal? custo = (custos.Fixo + custos.Variavel) * query.Max(q => q.Extensao);
              try {
                current.IndiceAtual = string.Format("{0:#,##0.000}", query.Sum(q => q.QtdViagens) * custo /
                                                                       query.Sum(q => q.Ajustado));
                current.DimensionaE = string.Format("{0:#,##0.000}", prognosticoE * custo / query.Sum(q => q.Ajustado));
                current.DimensionaP = string.Format("{0:#,##0.000}", prognosticoP * custo / query.Sum(q => q.Ajustado));
              }
              catch (DivideByZeroException ex) {
                throw new Exception(ex.Message);
              }
              try {
                current.EvolucaoE = -(1 - (prognosticoE * custo / query.Sum(q => q.Ajustado) /
                                            (query.Sum(q => q.QtdViagens) * custo / query.Sum(q => q.Ajustado))));
                current.EvolucaoP = -(1 - (prognosticoP * custo / query.Sum(q => q.Ajustado) /
                                            (query.Sum(q => q.QtdViagens) * custo / query.Sum(q => q.Ajustado))));
              }
              catch (DivideByZeroException ex) {
                throw new Exception(ex.Message);
              }
            }
            break;
        }
      }
      return current;
    }

    private Dimensionamento GetPeriodoPico(Expression<Func<Dimensionamento, bool>> where) {
      using (Services<Dimensionamento> source = new Services<Dimensionamento>()) {
        IQueryable<Dimensionamento> dataset = source.GetQuery(where);
        return dataset.FirstOrDefault(q => q.Ajustado == dataset.Max(p => p.Ajustado));
      }
    }

    private int GetTempoCiclo(Expression<Func<Dimensionamento, bool>> where) {
      using (Services<Dimensionamento> source = new Services<Dimensionamento>()) {
        int[] ciclo = new int[] { 0, 0 };

        IQueryable<Dimensionamento> dataset = source.GetQuery(where).Where(q => q.Sentido.Equals("AB"));
        int total = dataset.Sum(p => p.QtdViagens * p.CicloAB);
        try {
          ciclo[0] = (int)Math.Ceiling((decimal)total / dataset.Sum(q => q.QtdViagens));
        }
        catch (DivideByZeroException ex) {
          throw new Exception(ex.Message);
        }

        dataset = source.GetQuery(where).Where(q => q.Sentido.Equals("BA"));
        total = dataset.Sum(p => p.QtdViagens * p.CicloBA);
        try {
          ciclo[1] = (int)Math.Ceiling((decimal)total / dataset.Sum(q => q.QtdViagens));
        }
        catch (DivideByZeroException ex) {
          throw new Exception(ex.Message);
        }

        return ciclo[0] + ciclo[1];
      }
    }

    private int GetMediaVeiculos(Expression<Func<Dimensionamento, bool>> where) {
      using (Services<Dimensionamento> source = new Services<Dimensionamento>()) {
        IQueryable<Dimensionamento> dataset = source.GetQuery(where);

        int total = dataset.Sum(p => p.QtdViagens * p.Veiculos);
        try {
          return (int)Math.Ceiling((decimal)total / dataset.Sum(q => q.QtdViagens));
        }
        catch (DivideByZeroException ex) {
          throw new Exception(ex.Message);
        }
      }
    }

    private int GetMediaVeiculosE(Expression<Func<Dimensionamento, bool>> where) {
      using (Services<Dimensionamento> source = new Services<Dimensionamento>()) {
        int viagens = 0;
        int total = 0;

        foreach (Dimensionamento item in source.GetQuery(where)) { 
          viagens += item.PrognosticoE ?? 0;
          total += (item.PrognosticoE ?? 0) * (item.VeiculosE ?? 0);
        }
        try {
          return (int)Math.Ceiling((decimal)total / viagens);
        }
        catch (DivideByZeroException ex) {
          throw new Exception(ex.Message);
        }
      }
    }

    private int GetMediaVeiculosP(Expression<Func<Dimensionamento, bool>> where) {
      using (Services<Dimensionamento> source = new Services<Dimensionamento>()) {
        int viagens = 0;
        int total = 0;
        
        foreach (Dimensionamento item in source.GetQuery(where)) { 
          viagens += item.PrognosticoP ?? 0;
          total += (item.PrognosticoP ?? 0) * (item.VeiculosP ?? 0);
        }
        try {
          return (int)Math.Ceiling((decimal)total / viagens);
        }
        catch (DivideByZeroException ex) {
          throw new Exception(ex.Message);
        }
      }
    }
  }
}
