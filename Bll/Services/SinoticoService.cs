using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Extensions;
using Dto.Models;

namespace Bll.Services {
  public class SinoticoService : Services<Sinotico> {
    private readonly int userId;

    public SinoticoService(int? _userId = null) {
      userId = _userId ?? 1;
    }

    protected override IQueryable<Sinotico> Get(Expression<Func<Sinotico, bool>> filter = null,
        Func<IQueryable<Sinotico>, IOrderedQueryable<Sinotico>> orderBy = null) {
      try {
        int[] companies = context.Set<EUsuario>().AsNoTracking()
                              .Where(u => (u.UsuarioId == userId) && u.Ativo)
                              .Select(u => u.EmpresaId).Distinct().ToArray();

        IQueryable<Sinotico> query = (from s in context.Sinoticos
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
    protected static Sinotico ComputeIndices(Sinotico current) {
      using (DimensionamentoService dimensionamento = new DimensionamentoService()) {
        Expression<Func<Dimensionamento, bool>> filter = d => (d.PesquisaId == current.PesquisaId) &&
                                                              (d.LinhaId == current.LinhaId) && (d.DiaId == current.DiaId);
        IQueryable<Dimensionamento> query = dimensionamento.GetQuery(filter);

        int[] prognostic = { 0, 0, 0 };
        int[] duracao = { 0, 0 };
        int[,] vehicles = { { 0, 0, 0 }, { 0, 0, 0 } };

        int passageiros = query.Sum(q => q.Ajustado);
        prognostic[0] = query.Sum(q => q.QtdViagens);
        foreach (Dimensionamento item in query) {
          prognostic[1] += item.PrognosticoE ?? 0;
          prognostic[2] += item.PrognosticoP ?? 0;

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

        foreach (Dimensionamento item in query.Where(d => d.PeriodoId == periodoId)) {
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

        decimal aux = vehicles[1, 0];
        try {
          vehicles[1, 0] = (int)Math.Ceiling(aux / prognostic[0]);
        }
        catch (DivideByZeroException) {
          vehicles[1, 0] = 1;
        }
        for (int j = 1; j < vehicles.GetLength(1); j++) {
          aux = vehicles[1, j];
          try {
            vehicles[1, j] = (int)Math.Ceiling(aux / prognostic[j - 1]);
          }
          catch (DivideByZeroException) {
            vehicles[1, j] = 1;
          }
        }

        decimal velocidade = 22.5m;
        if (duracao[0] > 0) { 
          velocidade = prognostic[0] * extensao / duracao[0] * 60;
        }

        decimal? custo;
        using (Services<Linha> linhas = new Services<Linha>()) {
          int empresaId = linhas.GetById(current.LinhaId).EmpresaId;

          using Services<CustoMod> custosMod = new Services<CustoMod>();
          custo = (custosMod.GetFirst(q => q.EmpresaId == empresaId)?.Fixo +
                     custosMod.GetFirst(q => q.EmpresaId == empresaId)?.Variavel) * extensao;
        }

        int? total;
        switch (current.SinoticoId) {
          case 1:      // Volume de Passageiros (Pass/dia)
            current.IndiceAtual = $"{passageiros:#,##0}";
            current.DimensionaE = current.IndiceAtual;
            current.DimensionaP = current.IndiceAtual;
            break;
          case 2:      // Numero de Viagens (Unidirecionais) (Viagens/dia)
            current.IndiceAtual = $"{prognostic[0]:#,##0}";
            current.DimensionaE = $"{prognostic[1]:#,##0}";
            current.DimensionaP = $"{prognostic[2]:#,##0}";
            try {
              current.EvolucaoE = Handler.NullIf(-(1 - ((decimal)prognostic[1] / prognostic[0])), 0);
              current.EvolucaoP = Handler.NullIf(-(1 - ((decimal)prognostic[2] / prognostic[0])), 0);
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
            current.IndiceAtual = $"{prognostic[0] * extensao:#,##0.0}";
            current.DimensionaE = $"{prognostic[1] * extensao:#,##0.0}";
            current.DimensionaP = $"{prognostic[2] * extensao:#,##0.0}";
            try {
              current.EvolucaoE = Handler.NullIf(-(1 - (prognostic[1] * extensao / 
                                                         (prognostic[0] * extensao))), 0);
              current.EvolucaoP = Handler.NullIf(-(1 - (prognostic[2] * extensao / 
                                                         (prognostic[0] * extensao))), 0);
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
              current.IndiceAtual = $"{(decimal?)query.Sum(q => q.Ociosidade) / duracao[0]:P1}";
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            try {
              current.DimensionaE = $"{Handler.NullIf(1 - prognostic[1] * extensao * 60 / velocidade / duracao[0], 0):P1}";
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            try {
              current.DimensionaP = $"{Handler.NullIf(1 - prognostic[2] * extensao * 60 / velocidade / duracao[0], 0):P1}";
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
              }
              catch (DivideByZeroException ex) {
                throw new Exception(ex.Message);
              }              
            }
            current.DimensionaE = current.IndiceAtual;
            current.DimensionaP = current.IndiceAtual;
            break;
          case 9:      // Velocidade Comercial (Media) (km/h)
            current.IndiceAtual = $"{velocidade:#,##0.0}";
            current.DimensionaE = current.IndiceAtual;
            current.DimensionaP = current.IndiceAtual;
            break;
          case 10:     // Frota Total (Pico)
            if (periodoId > 0) {
              current.IndiceAtual = $"{vehicles[0, 0]:#,##0}";
              current.DimensionaE = $"{vehicles[0, 1]:#,##0}";
              current.DimensionaP = $"{vehicles[0, 2]:#,##0}";
              try {
                current.EvolucaoE = Handler.NullIf(-(1 - (decimal)vehicles[0, 1] / vehicles[0, 0]), 0);
                current.EvolucaoP = Handler.NullIf(-(1 - (decimal)vehicles[0, 2] / vehicles[0, 0]), 0);
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
              current.EvolucaoE = Handler.NullIf(-(1 - (decimal)vehicles[1, 1] / vehicles[1, 0]), 0);
              current.EvolucaoP = Handler.NullIf(-(1 - (decimal)vehicles[1, 2] / vehicles[1, 0]), 0);
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            break;
          case 12:     // Custo Operacional (R$)           
            current.IndiceAtual = $"{prognostic[0] * custo:C2}";
            current.DimensionaE = $"{prognostic[1] * custo:C2}";
            current.DimensionaP = $"{prognostic[2] * custo:C2}";
            try {
              current.EvolucaoE = Handler.NullIf(-(1 - ((decimal)prognostic[1] / prognostic[0])), 0);
              current.EvolucaoP = Handler.NullIf(-(1 - ((decimal)prognostic[2] / prognostic[0])), 0);
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            break;
          case 13:     // Taxa de Utilizacao (IPK) (Pass/km)
            try {
              current.IndiceAtual = $"{passageiros / (prognostic[0] * extensao):#,##0.000}";
              current.DimensionaE = $"{passageiros / (prognostic[1] * extensao):#,##0.000}";
              current.DimensionaP = $"{passageiros / (prognostic[2] * extensao):#,##0.000}";
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            try {
              current.EvolucaoE = Handler.NullIf(-(1 - passageiros / (prognostic[1] * extensao) /
                                                         (passageiros / (prognostic[0] * extensao))), 0);
              current.EvolucaoP = Handler.NullIf(-(1 - passageiros / (prognostic[2] * extensao) /
                                                         (passageiros / (prognostic[0] * extensao))), 0);
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            break;
          case 14:     // Uso da Frota	(km/veic)
            try {
              current.IndiceAtual = $"{prognostic[0] * extensao / vehicles[1, 0]:#,##0.0}";
              current.DimensionaE = $"{prognostic[1] * extensao / vehicles[1, 1]:#,##0.0}";
              current.DimensionaP = $"{prognostic[2] * extensao / vehicles[1, 2]:#,##0.0}";
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            try {
              current.EvolucaoE = Handler.NullIf(-(1 - prognostic[1] * extensao / vehicles[1, 1] /
                                                         (prognostic[0] * extensao / vehicles[1, 0])), 0);
              current.EvolucaoP = Handler.NullIf(-(1 - prognostic[2] * extensao / vehicles[1, 2] /
                                                         (prognostic[0] * extensao / vehicles[1, 0])), 0);
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            break;
          case 15:     // Rendimento da Frota (Pass/veic)
            try {
              current.IndiceAtual = $"{passageiros / vehicles[1, 0]:#,##0}";
              current.DimensionaE = $"{passageiros / vehicles[1, 1]:#,##0}";
              current.DimensionaP = $"{passageiros / vehicles[1, 2]:#,##0}";
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            try {
              current.EvolucaoE = Handler.NullIf(-(1 - passageiros / vehicles[1, 1] /
                                                         (passageiros / vehicles[1, 0])), 0);
              current.EvolucaoP = Handler.NullIf(-(1 - passageiros / vehicles[1, 2] /
                                                         (passageiros / vehicles[1, 0])), 0);
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            break;
          case 16:     // Custo do Transporte (R$/pass)
            try {
              current.IndiceAtual = $"{prognostic[0] * custo / passageiros:C2}";
              current.DimensionaE = $"{prognostic[1] * custo / passageiros:C2}";
              current.DimensionaP = $"{prognostic[2] * custo / passageiros:C2}";
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }
            try {                
              current.EvolucaoE = Handler.NullIf(-(1 - (prognostic[1] * aux / (prognostic[0] * aux))), 0);
              current.EvolucaoP = Handler.NullIf(-(1 - (prognostic[2] * aux / (prognostic[0] * aux))), 0);
            }
            catch (DivideByZeroException ex) {
              throw new Exception(ex.Message);
            }            
            break;
          }
      }
      return current;
    }
  }
}
