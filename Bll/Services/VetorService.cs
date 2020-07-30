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
      Premissa premissa = await premissas.GetFirstAsync(p => p.EmpresaId == companyId);
      if (premissa == null) {
        return;
      }
      int interval = (int)Math.Ceiling(premissa.JornadaDia * 60 * premissa.VetorPadrao);
      int jornada = (int)Math.Ceiling(premissa.JornadaDia * 60);

      IQueryable<Vetor> query;
      while (await (query = Get(q => q.EmpresaId == companyId,
                                q => q.OrderBy(m => m.DiaId).ThenBy(m => m.Inicio))).CountAsync() > 0) {
        Vetor next = await query.FirstAsync();
        Arco arco = new Arco() {
            EmpresaId = next.EmpresaId, DiaId = next.DiaId,
            Inicio = next.Inicio, PInicioId = next.PInicioId,
            Termino = next.Termino, PTerminoId = next.PTerminoId
        };

        int itemId = 0;
        using (Services<Arco> service = new Services<Arco>()) {
          await service.Insert(arco);

          using Services<ArcoV> hArco = new Services<ArcoV>();
          await hArco.Insert(new ArcoV() {
              ArcoId = arco.Id, Item = ++itemId, VetorId = next.Id
          });
        }
        int runtime = CustomCalendar.Runtime(arco.Inicio, arco.Termino);
        if (runtime >= jornada) {
          continue;
        }
        int period = runtime;

        if (period >= interval) {
          arco.Termino = arco.Termino.Add(TimeSpan.FromMinutes((int)premissa.IntraJornadaMin * 60));
          period = 0;
        }
        TimeSpan toStart = arco.Termino.Add(TimeSpan.FromMinutes((int)premissa.Deslocamento));
        if (toStart.Days > arco.Termino.Days) {
          toStart = arco.Termino;
        }

        IList<int> pontos = new List<int>();
        using Services<Adjacencia> adjacentes = new Services<Adjacencia>();
        if (await adjacentes.ExistsAsync(p => p.PontoId == next.PTerminoId)) {
          if (toStart.Add(TimeSpan.FromMinutes((int)premissa.Deslocamento)).Days > toStart.Days) {
            continue;
          }
          next.Termino = toStart;
          toStart = next.Termino.Add(TimeSpan.FromMinutes((int)premissa.Deslocamento));
          pontos = await adjacentes.GetQuery(q => q.PontoId == next.PTerminoId)
                             .Select(q => q.AdjacenteId).Distinct().ToListAsync();
        }
        pontos.Add((int)next.PTerminoId);

        while (await (query = (from v in context.Vetores
                               where v.EmpresaId == companyId && v.DiaId == next.DiaId &&
                                     v.Inicio >= next.Termino && v.Inicio <= toStart &&
                                     pontos.Contains((int)v.PInicioId)
                               orderby v.Inicio, v.Duracao descending
                               select v).Include(v => v.PInicio).Include(v => v.PTermino)).CountAsync() > 0) {
          next = await query.FirstAsync();
          using Services<ArcoV> hArco = new Services<ArcoV>();
          await hArco.Insert(new ArcoV {
            ArcoId = arco.Id, Item = ++itemId, VetorId = next.Id
          });

          arco.Termino = next.Termino;
          arco.PTerminoId = next.PTerminoId;
          using (Services<Arco> service = new Services<Arco>()) {
            await service.Update(arco);
          }
          runtime += CustomCalendar.Runtime(arco.Inicio, arco.Termino);          
          if (runtime >= jornada) {
            break;
          }

          period += CustomCalendar.Runtime(arco.Inicio, arco.Termino);
          if (period >= interval) {
            arco.Termino = arco.Termino.Add(TimeSpan.FromMinutes((int)premissa.IntraJornadaMin * 60));
          }
          toStart = arco.Termino.Add(TimeSpan.FromMinutes((int)premissa.Deslocamento));
          if (toStart.Days > arco.Termino.Days) {
            toStart = arco.Termino;
          }

          pontos.Clear();
          if (await adjacentes.ExistsAsync(p => p.PontoId == next.PTerminoId)) {
            if (toStart.Add(TimeSpan.FromMinutes((int)premissa.Deslocamento)).Days > toStart.Days) {
              break;
            }
            next.Termino = toStart;
            toStart = next.Termino.Add(TimeSpan.FromMinutes((int)premissa.Deslocamento));
            pontos = await adjacentes.GetQuery(q => q.PontoId == next.PTerminoId)
                               .Select(q => q.AdjacenteId).Distinct().ToListAsync();
          }
          pontos.Add((int)next.PTerminoId);
        }
      }
    }
  }
}
