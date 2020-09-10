using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Dto.Extensions;
using Dto.Models;

namespace Bll.Services {
  public class MatrizHService : Services<MatrizH> {
    protected override IQueryable<MatrizH> Get(Expression<Func<MatrizH, bool>> filter = null, 
        Func<IQueryable<MatrizH>, IOrderedQueryable<MatrizH>> orderBy = null) {
      return base.Get(filter, orderBy).Include(m => m.PInicio).Include(m => m.PTermino);
    }

    public async Task Vectorize(int companyId) {
      using Services<Premissa> premissas = new Services<Premissa>();
      Premissa premissa = premissas.GetFirst(p => p.EmpresaId == companyId);
      if (premissa == null) {
        return;
      }
      int interval = premissa.VetorPadrao;

      IQueryable<MatrizH> query;      
      while ((query = Get(q => q.EmpresaId == companyId, 
                          q => q.OrderBy(m => m.DiaId).ThenBy(m => m.Item)
                                .ThenBy(m => m.Inicio))).Count() > 0) {
        MatrizH next = query.First();
        if ((next.Item == 1) && !next.PInicio.Garagem) {
          if (next.Inicio.Subtract(TimeSpan.FromMinutes(premissa.DeslocaInicial.Value)).Days == next.Inicio.Days) {
            next.Inicio = next.Inicio.Subtract(TimeSpan.FromMinutes(premissa.DeslocaInicial.Value));
          }
          else {
            next.Inicio = new TimeSpan();
          }
        }
        Vetor vector = new Vetor() { 
            EmpresaId = next.EmpresaId, DiaId = next.DiaId, Inicio = next.Inicio, 
            PInicioId = next.PInicioId, Termino = next.Termino, PTerminoId = next.PTerminoId
        };

        int itemId = 0;
        using (Services<Vetor> service = new Services<Vetor>()) {
          await service.Insert(vector);

          using Services<VetorH> vetorh = new Services<VetorH>();
          await vetorh.Insert(new VetorH() {
                    Item = ++itemId, VetorId = vector.Id, HorarioId = next.Id
                });
        }
        int runtime = CustomCalendar.Runtime(vector.Inicio, vector.Termino);
        if ((runtime >= interval) && next.PTermino.Intercambio) {
          continue;
        }
        
        if (next.Termino.Add(TimeSpan.FromMinutes(premissa.Deslocamento.Value)).Days > next.Termino.Days) {
          next.Termino = new TimeSpan();
        }
        TimeSpan maxInicio = next.Termino.Add(TimeSpan.FromMinutes(premissa.Deslocamento.Value));

        while ((query = Get(q => (q.EmpresaId == companyId) && (q.DiaId == next.DiaId) &&
                                 (q.Inicio >= next.Termino) && (q.Inicio <= maxInicio) && (q.PInicioId == next.PTerminoId),
                            q => q.OrderBy(p => p.Inicio).ThenByDescending(p => p.Duracao))).Count() > 0) {
          next = query.First();
          using Services<VetorH> vetorh = new Services<VetorH>();
          await vetorh.Insert(new VetorH {
              Item = ++itemId, VetorId = vector.Id, HorarioId = next.Id
          });

          vector.Termino = next.Termino;
          vector.PTerminoId = next.PTerminoId;
          using (Services<Vetor> service = new Services<Vetor>()) {
            await service.Update(vector);
          }
          runtime = CustomCalendar.Runtime(vector.Inicio, vector.Termino);
          if ((runtime >= interval) && next.PTermino.Intercambio) {
            break;
          }

          if (next.Termino.Add(TimeSpan.FromMinutes(premissa.Deslocamento.Value)).Days > next.Termino.Days) {
            next.Termino = new TimeSpan();
          }
          maxInicio = next.Termino.Add(TimeSpan.FromMinutes(premissa.Deslocamento.Value));
        }
      }
    }
  }
}
