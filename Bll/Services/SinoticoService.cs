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
                                      join p in context.Pesquisas on s.PesquisaId equals p.Id
                                      join l in context.Linhas on s.LinhaId equals l.Id
                                      where companies.Contains(l.EmpresaId)
                                      orderby l.EmpresaId, s.LinhaId, s.DiaId, s.SinoticoId
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
      ISet<Sinotico> result = new HashSet<Sinotico>();
      foreach (Sinotico item in Get()) {
        result.Add(ComputeIndices(item));
      }
      return result.AsQueryable();
    }

    public IQueryable<Sinotico> GetQuery(Expression<Func<Sinotico, bool>> filter = null,
        Func<IQueryable<Sinotico>, IOrderedQueryable<Sinotico>> orderBy = null) {
      ISet<Sinotico> result = new HashSet<Sinotico>();
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
      using (DimensionamentoService dimensionamento = new DimensionamentoService()) {
        Expression<Func<Dimensionamento, bool>> filter = d => (d.PesquisaId == current.PesquisaId) &&
                                                              (d.LinhaId == current.LinhaId) && (d.DiaId == current.DiaId);
        IQueryable<Dimensionamento> query = dimensionamento.GetQuery(filter);

        int[,] prognostic = new int[2, 2] { { 0, 0 }, { 0, 0 } };
        int[,] vehicles = new int[2, 3] { { 0, 0, 0 }, { 0, 0, 0 } };
        int[] duracao = new int[2] { 0, 0 };

        foreach (Dimensionamento item in query) {
          prognostic[0, 0] += item.PrognosticoE ?? 0;
          prognostic[0, 1] += item.PrognosticoP ?? 0;

          vehicles[1, 0] += item.QtdViagens * (item.Veiculos ?? 1);
          vehicles[1, 1] += (item.PrognosticoE ?? 0) * (item.VeiculosE ?? 1);
          vehicles[1, 2] += (item.PrognosticoP ?? 0) * (item.VeiculosP ?? 1);
        }
        decimal extensao = dimensionamento.Extensao(filter);

        duracao[0] = dimensionamento.TempoTotal(filter);

        int periodoId = 0;
        foreach (int pico in query.Select(d => d.PeriodoId).Distinct().ToArray()) {
          using Services<PrLinha> prLinhas = new Services<PrLinha>();
          PrLinha prLinha = prLinhas.GetById(pico);
          if (prLinha.EPeriodo.Pico) {
            periodoId = prLinha.Id;
            break;
          }
        }

        if (periodoId > 0) {
          foreach (Dimensionamento item in query.Where(d => d.PeriodoId == periodoId)) {
            prognostic[1, 0] += item.PrognosticoE ?? 0;
            prognostic[1, 1] += item.PrognosticoP ?? 0;

            if ((item.Veiculos ?? 1) > vehicles[0, 0]) {
              vehicles[0, 0] = item.Veiculos ?? 1;
            }
            if ((item.VeiculosE ?? 1) > vehicles[0, 1]) {
              vehicles[0, 1] = item.VeiculosE ?? 1;
            }
            if ((item.VeiculosP ?? 1) > vehicles[0, 2]) {
              vehicles[0, 2] = item.VeiculosP ?? 1;
            }
          }
          duracao[1] = dimensionamento.TempoTotal(d => (d.PesquisaId == current.PesquisaId) &&
                                                       (d.LinhaId == current.LinhaId) &&
                                                       (d.DiaId == current.DiaId) && (d.PeriodoId == periodoId));
        }

        decimal aux = vehicles[1, 0];
        try {
          vehicles[1, 0] = (int)Math.Ceiling(aux / query.Sum(q => q.QtdViagens));
        }
        catch (DivideByZeroException) {
          vehicles[1, 0] = 1;
        }
        for (int j = 1; j < vehicles.GetLength(1); j++) {
          aux = vehicles[1, j];
          try {
            vehicles[1, j] = (int)Math.Ceiling(aux / prognostic[0, j - 1]);
          }
          catch (DivideByZeroException) {
            vehicles[1, j] = 1;
          }
        }

        using Services<Linha> linhas = new Services<Linha>();
        int empresaId = linhas.GetById(current.LinhaId).EmpresaId;

        using Services<CustoMod> custosMod = new Services<CustoMod>();
        CustoMod custos = custosMod.GetFirst(q => q.EmpresaId == empresaId);

        int? total;
        switch (current.SinoticoId) {
          case 1:      // Volume de Passageiros (Pass/dia)
            current.IndiceAtual = $"{query.Sum(q => q.Ajustado):#,##0}";
            current.DimensionaE = current.IndiceAtual;
            current.DimensionaP = current.IndiceAtual;
            break;
          case 2:      // Numero de Viagens (Unidirecionais) (Viagens/dia)
            current.IndiceAtual = $"{query.Sum(q => q.QtdViagens):#,##0}";
            current.DimensionaE = $"{prognostic[0, 0]:#,##0}";
            current.DimensionaP = $"{prognostic[0, 1]:#,##0}";
            try {
              current.EvolucaoE = -(1 - ((decimal)prognostic[0, 0] / query.Sum(q => q.QtdViagens)));
              current.EvolucaoP = -(1 - ((decimal)prognostic[0, 1] / query.Sum(q => q.QtdViagens)));
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }            
            break;
          case 3:      // Extensao das Viagens (km)
            current.IndiceAtual = $"{extensao:#,##0.0}";
            current.DimensionaE = current.IndiceAtual;
            current.DimensionaP = current.IndiceAtual;
            break;
          case 4:      // Quilometragem Percorrida (km/dia)
            current.IndiceAtual = $"{query.Sum(q => q.QtdViagens) * extensao:#,##0.0}";
            current.DimensionaE = $"{prognostic[0, 0] * extensao:#,##0.0}";
            current.DimensionaP = $"{prognostic[0, 1] * extensao:#,##0.0}";
            try {
              current.EvolucaoE = -(1 - (prognostic[0, 0] * extensao /
                                          (query.Sum(q => q.QtdViagens) * extensao)));
              current.EvolucaoP = -(1 - (prognostic[0, 1] * extensao /
                                          (query.Sum(q => q.QtdViagens) * extensao)));
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }            
            break;
          case 5:      // Pontos de Parada
            using (Services<PtLinha> pontos = new Services<PtLinha>()) {
              total = pontos.GetCount(q => q.LinhaId == current.LinhaId);
            }
            current.IndiceAtual = $"{total:#,###}";
            current.DimensionaE = current.IndiceAtual;
            current.DimensionaP = current.IndiceAtual;
            break;
          case 6:      // Tempo de Ciclo (Min.)
            current.IndiceAtual = $"{dimensionamento.TempoViagem(filter):#,##0}";
            current.DimensionaE = current.IndiceAtual;
            current.DimensionaP = current.IndiceAtual;
            break;
          case 7:      // Ociosidade (%)
            try {
              current.IndiceAtual = $"{(decimal?)query.Sum(q => q.Ociosidade) / duracao[0]:P2}";
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            break;
          case 8:      // Velocidade Comercial (Pico) (km/h)
            if (periodoId > 0) {
              total = query.Where(d => d.PeriodoId == periodoId).Sum(d => d.QtdViagens);
              try {
                current.IndiceAtual = $"{total * extensao / duracao[1] * 60:#,##0.0}";
                current.DimensionaE = $"{prognostic[1, 0] * extensao / duracao[1] * 60:#,##0.0}";
                current.DimensionaP = $"{prognostic[1, 1] * extensao / duracao[1] * 60:#,##0.0}";
              }
              catch (DivideByZeroException ex) {
                throw new Exception(ex.Message);
              }
              try {
                current.EvolucaoE = -(1 - prognostic[1, 0] * extensao / duracao[1] * 60 /
                                            (total * extensao / duracao[1] * 60));
                current.EvolucaoP = -(1 - prognostic[1, 1] * extensao / duracao[1] * 60 /
                                            (total * extensao / duracao[1] * 60));
              }
              catch (DivideByZeroException ex) {
                throw new Exception(ex.Message);
              }
            }
            break;
          case 9:      // Velocidade Comercial (Media) (km/h)
            try {
              current.IndiceAtual = $"{query.Sum(q => q.QtdViagens) * extensao / duracao[0] * 60:#,##0.0}";
              current.DimensionaE = $"{prognostic[0, 0] * extensao / duracao[0] * 60:#,##0.0}";
              current.DimensionaP = $"{prognostic[0, 1] * extensao / duracao[0] * 60:#,##0.0}";
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            try {
              current.EvolucaoE = -(1 - prognostic[0, 0] * extensao / duracao[0] * 60 /
                                          (query.Sum(q => q.QtdViagens) * extensao / duracao[0] * 60));
              current.EvolucaoP = -(1 - prognostic[0, 1] * extensao / duracao[0] * 60 /
                                          (query.Sum(q => q.QtdViagens) * extensao / duracao[0] * 60));
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            break;
          case 10:     // Frota Total (Pico)
            if (periodoId > 0) {
              current.IndiceAtual = $"{vehicles[0, 0]:#,##0}";
              current.DimensionaE = $"{vehicles[0, 1]:#,##0}";
              current.DimensionaP = $"{vehicles[0, 2]:#,##0}";
              try {
                current.EvolucaoE = -(1 - (decimal)vehicles[0, 1] / vehicles[0, 0]);
                current.EvolucaoP = -(1 - (decimal)vehicles[0, 2] / vehicles[0, 0]);
              }
              catch (DivideByZeroException ex) {
                throw new Exception(ex.Message);
              }
            }
            break;
          case 11:     // Frota Total (Media)
            current.IndiceAtual = $"{vehicles[1, 0]:#,##0}";
            current.DimensionaE = $"{vehicles[1, 1]:#,##0}";
            current.DimensionaP = $"{vehicles[1, 2]:#,##0}";
            try {
              current.EvolucaoE = -(1 - (decimal)vehicles[1, 1] / vehicles[1, 0]);
              current.EvolucaoP = -(1 - (decimal)vehicles[1, 2] / vehicles[1, 0]);
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            break;
          case 12:     // Custo Operacional (R$)
            if (custos != null) {
              decimal? custo = (custos.Fixo + custos.Variavel) * extensao;
              current.IndiceAtual = $"{query.Sum(q => q.QtdViagens) * custo:C2}";
              current.DimensionaE = $"{prognostic[0, 0] * custo:C2}";
              current.DimensionaP = $"{prognostic[0, 1] * custo:C2}";

              try {
                current.EvolucaoE = -(1 - ((decimal)prognostic[0, 0] / query.Sum(q => q.QtdViagens)));
                current.EvolucaoP = -(1 - ((decimal)prognostic[0, 1] / query.Sum(q => q.QtdViagens)));
              }
              catch (DivideByZeroException ex) {
                throw new Exception(ex.Message);
              }
            }
            break;
          case 13:     // Taxa de Utilizacao (IPK) (Pass/km)
            try {
              current.IndiceAtual = $"{query.Sum(q => q.Ajustado) / (query.Sum(q => q.QtdViagens) * extensao):#,##0.000}";
              current.DimensionaE = $"{query.Sum(q => q.Ajustado) / (prognostic[0, 0] * extensao):#,##0.000}";
              current.DimensionaP = $"{query.Sum(q => q.Ajustado) / (prognostic[0, 1] * extensao):#,##0.000}";
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            try {
              current.EvolucaoE = -(1 - query.Sum(q => q.Ajustado) / (prognostic[0, 0] * extensao) /
                                          (query.Sum(q => q.Ajustado) / 
                                            (query.Sum(q => q.QtdViagens) * extensao)));
              current.EvolucaoP = -(1 - query.Sum(q => q.Ajustado) / (prognostic[0, 1] * extensao) /
                                          (query.Sum(q => q.Ajustado) /
                                            (query.Sum(q => q.QtdViagens) * extensao)));
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            break;
          case 14:     // Uso da Frota	(km/veic)
            try {
              current.IndiceAtual = $"{query.Sum(q => q.QtdViagens) * extensao / vehicles[1, 0]:#,##0.0}";
              current.DimensionaE = $"{prognostic[0, 0] * extensao / vehicles[1, 1]:#,##0.0}";
              current.DimensionaP = $"{prognostic[0, 1] * extensao / vehicles[1, 2]:#,##0.0}";
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            try {
              current.EvolucaoE = -(1 - prognostic[0, 0] * extensao / vehicles[1, 1] /
                                          (query.Sum(q => q.QtdViagens) * extensao / vehicles[1, 0]));
              current.EvolucaoP = -(1 - prognostic[0, 1] * extensao / vehicles[1, 2] /
                                          (query.Sum(q => q.QtdViagens) * extensao / vehicles[1, 0]));
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            break;
          case 15:     // Rendimento da Frota (Pass/veic)
            try {
              current.IndiceAtual = $"{query.Sum(q => q.Ajustado) / vehicles[1, 0]:#,##0}";
              current.DimensionaE = $"{query.Sum(q => q.Ajustado) / vehicles[1, 1]:#,##0}";
              current.DimensionaP = $"{query.Sum(q => q.Ajustado) / vehicles[1, 2]:#,##0}";
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            try {
              current.EvolucaoE = -(1 - query.Sum(q => q.Ajustado) / vehicles[1, 1] /
                                          (query.Sum(q => q.Ajustado) / vehicles[1, 0]));
              current.EvolucaoP = -(1 - query.Sum(q => q.Ajustado) / vehicles[1, 2] /
                                          (query.Sum(q => q.Ajustado) / vehicles[1, 0]));
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            break;
          case 16:     // Custo do Transporte (R$/pass)
            if (custos != null) {
              decimal custo = (custos.Fixo + custos.Variavel) * extensao;
              
              aux = custo / query.Sum(q => q.Ajustado);
              try {
                current.IndiceAtual = $"{query.Sum(q => q.QtdViagens) * aux:C3}";
                current.DimensionaE = $"{prognostic[0, 0] * aux:C3}";
                current.DimensionaP = $"{prognostic[0, 1] * aux:C3}";
              }
              catch (DivideByZeroException ex) {
                throw new Exception(ex.Message);
              }
              try {                
                current.EvolucaoE = -(1 - (prognostic[0, 0] * aux / (query.Sum(q => q.QtdViagens) * aux)));
                current.EvolucaoP = -(1 - (prognostic[0, 1] * aux / (query.Sum(q => q.QtdViagens) * aux)));
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
  }
}
