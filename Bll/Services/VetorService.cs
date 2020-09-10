using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Dto.Extensions;
using Dto.Models;

namespace Bll.Services {
  public class VetorService : Services<Vetor> {
    protected override IQueryable<Vetor> Get(Expression<Func<Vetor, bool>> filter = null,
        Func<IQueryable<Vetor>, IOrderedQueryable<Vetor>> orderBy = null) {
      return base.Get(filter, orderBy).Include(v => v.PInicio).Include(v => v.PTermino);
    }

    public async Task DoArcs(int companyId) {
      using Services<Premissa> premissas = new Services<Premissa>();
      Premissa premissa = premissas.GetFirst(p => p.EmpresaId == companyId);
      if (premissa == null) {
        return;
      }
      int jornada = (int)Math.Ceiling(premissa.JornadaDia * 60);
      int breaktime = (int)Math.Ceiling(premissa.IntraJornadaMin * 60);
      int interval = premissa.VetorPadrao;

      IQueryable<Vetor> query;
      while ((query = Get(q => q.EmpresaId == companyId, 
                          q => q.OrderBy(m => m.DiaId).ThenBy(m => m.Id))).Count() > 0) {
        Vetor next = query.First();
        Arco arco = new Arco() {
            EmpresaId = next.EmpresaId, DiaId = next.DiaId, Inicio = next.Inicio,
            PInicioId = next.PInicioId, Termino = next.Termino, PTerminoId = next.PTerminoId
        };        

        int itemId = 0;
        using (Services<Arco> service = new Services<Arco>()) {
          await service.Insert(arco);

          using Services<ArcoV> arcoV = new Services<ArcoV>();
          await arcoV.Insert(new ArcoV() {
              Item = ++itemId, ArcoId = arco.Id, VetorId = next.Id
          });
        }
        bool hasBreak = false;
        int runtime = next.Duracao.Value;

        Expression<Func<Vetor, bool>> condition = q => (q.EmpresaId == companyId) && (q.DiaId == next.DiaId);
        TimeSpan maxInicio;
        if (runtime < interval) {
          if (next.Termino.Add(TimeSpan.FromMinutes(premissa.Deslocamento.Value)).Days > next.Termino.Days) {
            next.Termino = new TimeSpan();
          }
          maxInicio = next.Termino.Add(TimeSpan.FromMinutes(premissa.Deslocamento.Value));
          
          if ((query = Get(Predicate.And(condition, 
                                         q => (q.Inicio >= next.Termino) && (q.Inicio <= maxInicio) && 
                                              (q.PInicioId == next.PTerminoId)),
                           q => q.OrderBy(p => p.Inicio).ThenBy(p => p.Duracao))).Count() == 0) {
            if (next.Termino.Add(TimeSpan.FromMinutes(breaktime * 0.5)).Days > next.Termino.Days) {
              next.Termino = new TimeSpan();
            }
            maxInicio = next.Termino.Add(TimeSpan.FromMinutes(breaktime * 0.5));

            IList<int> pontos = new List<int>();
            using (Services<Adjacencia> adjacentes = new Services<Adjacencia>()) {
              pontos = adjacentes.GetQuery(q => q.PontoId == next.PTerminoId)
                           .Select(q => q.AdjacenteId).Distinct().ToList();
            }
            pontos.Add(next.PTerminoId.Value);
            query = (from m in context.Vetores
                     where m.EmpresaId == companyId && m.DiaId == next.DiaId &&
                           m.Inicio >= next.Termino && m.Inicio <= maxInicio && pontos.Contains(m.PInicioId.Value)
                     orderby m.Inicio, m.Duracao
                     select m);
          }
        }
        else {
          int addInterval = (!hasBreak) ? breaktime : premissa.Deslocamento.Value;
          if (next.Termino.Add(TimeSpan.FromMinutes(addInterval)).Days != next.Termino.Days) {
            continue;
          }
          else {
            if (!hasBreak) {
              next.Termino = next.Termino.Add(TimeSpan.FromMinutes(breaktime));
            }
            if (next.Termino.Add(TimeSpan.FromMinutes(addInterval)).Days == next.Termino.Days) {
              maxInicio = next.Termino.Add(TimeSpan.FromMinutes(addInterval));
            }
            else {
              maxInicio = new TimeSpan().Subtract(TimeSpan.FromSeconds(1));
            }
            
            if ((query = Get(Predicate.And(condition,
                                           q => (q.Inicio >= next.Termino) && (q.Inicio <= maxInicio) &&
                                                (q.PInicioId == next.PTerminoId)),
                             q => q.OrderBy(p => p.Inicio).ThenBy(p => p.Duracao))).Count() == 0) {
              if (maxInicio.Add(TimeSpan.FromMinutes(breaktime * 0.5)).Days == maxInicio.Days) {
                maxInicio = maxInicio.Add(TimeSpan.FromMinutes(breaktime * 0.5));
              }

              IList<int> pontos = new List<int>();
              using (Services<Adjacencia> adjacentes = new Services<Adjacencia>()) {
                pontos = adjacentes.GetQuery(q => (q.EmpresaId == companyId) && (q.PontoId != next.PTerminoId))
                             .Select(q => q.AdjacenteId).Distinct().ToList();
              }
              pontos.Add(next.PTerminoId.Value);
              query = (from m in context.Vetores
                       where m.EmpresaId == companyId && m.DiaId == next.DiaId &&
                             m.Inicio >= next.Termino && m.Inicio <= maxInicio && pontos.Contains(m.PInicioId.Value)
                       orderby m.Inicio, m.Duracao
                       select m);
            }
          }
          hasBreak = true;
        }

        while ((query.Count() > 0) && (runtime < jornada)) {
          next = query.First();
          using Services<ArcoV> arcoV = new Services<ArcoV>();
          await arcoV.Insert(new ArcoV {
              Item = ++itemId, ArcoId = arco.Id, VetorId = next.Id
          });

          arco.Termino = next.Termino;
          arco.PTerminoId = next.PTerminoId;
          using (Services<Arco> service = new Services<Arco>()) {
            await service.Update(arco);
          }
          runtime += next.Duracao.Value;

          if (runtime < interval) {
            if (next.Termino.Add(TimeSpan.FromMinutes(premissa.Deslocamento.Value)).Days > next.Termino.Days) {
              next.Termino = new TimeSpan();
            }
            maxInicio = next.Termino.Add(TimeSpan.FromMinutes(premissa.Deslocamento.Value));
              
            if ((query = Get(Predicate.And(condition,
                                           q => (q.Inicio >= next.Termino) && (q.Inicio <= maxInicio) &&
                                                (q.PInicioId == next.PTerminoId)),
                             q => q.OrderBy(p => p.Inicio).ThenBy(p => p.Duracao))).Count() == 0) {
              if (next.Termino.Add(TimeSpan.FromMinutes(breaktime * 0.5)).Days > next.Termino.Days) {
                next.Termino = new TimeSpan();
              }
              maxInicio = next.Termino.Add(TimeSpan.FromMinutes(breaktime * 0.5));

              IList<int> pontos = new List<int>();
              using (Services<Adjacencia> adjacentes = new Services<Adjacencia>()) {
                pontos = adjacentes.GetQuery(q => q.PontoId == next.PTerminoId)
                             .Select(q => q.AdjacenteId).Distinct().ToList();
              }
              pontos.Add(next.PTerminoId.Value);
              query = (from m in context.Vetores
                       where m.EmpresaId == companyId && m.DiaId == next.DiaId &&
                             m.Inicio >= next.Termino && m.Inicio <= maxInicio && pontos.Contains(m.PInicioId.Value)
                       orderby m.Inicio, m.Duracao
                       select m);
            }
          }
          else {
            int addInterval = (!hasBreak) ? breaktime : premissa.Deslocamento.Value;
            if (next.Termino.Add(TimeSpan.FromMinutes(addInterval)).Days != next.Termino.Days) {
              break;
            }
            else {
              if (!hasBreak) {
                next.Termino = next.Termino.Add(TimeSpan.FromMinutes(breaktime));
              }
              if (next.Termino.Add(TimeSpan.FromMinutes(addInterval)).Days == next.Termino.Days) {
                maxInicio = next.Termino.Add(TimeSpan.FromMinutes(addInterval));
              }
              else {
                maxInicio = new TimeSpan().Subtract(TimeSpan.FromSeconds(1));
              }
                
              if ((query = Get(Predicate.And(condition,
                                             q => (q.Inicio >= next.Termino) && (q.Inicio <= maxInicio) &&
                                                  (q.PInicioId == next.PTerminoId)),
                               q => q.OrderBy(p => p.Inicio).ThenBy(p => p.Duracao))).Count() == 0) {
                if (maxInicio.Add(TimeSpan.FromMinutes(breaktime * 0.5)).Days == maxInicio.Days) {
                  maxInicio = maxInicio.Add(TimeSpan.FromMinutes(breaktime * 0.5));
                }

                IList<int> pontos = new List<int>();
                using (Services<Adjacencia> adjacentes = new Services<Adjacencia>()) {
                  pontos = adjacentes.GetQuery(q => (q.EmpresaId == companyId) && (q.PontoId != next.PTerminoId))
                               .Select(q => q.AdjacenteId).Distinct().ToList();
                }
                pontos.Add(next.PTerminoId.Value);
                query = (from m in context.Vetores
                         where m.EmpresaId == companyId && m.DiaId == next.DiaId &&
                               m.Inicio >= next.Termino && m.Inicio <= maxInicio && pontos.Contains(m.PInicioId.Value)
                         orderby m.Inicio, m.Duracao
                         select m);
              }
              hasBreak = true;
            }
          }         
        }
      }
    }
  }
}
