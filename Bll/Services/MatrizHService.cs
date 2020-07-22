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
      static IOrderedQueryable<MatrizH> order(IQueryable<MatrizH> q) => q.OrderBy(m => m.DiaId)
                                                                         .ThenBy(m => m.Item).ThenBy(m => m.Inicio);
      Expression<Func<MatrizH, bool>> condition = q => q.EmpresaId == companyId;

      IQueryable<MatrizH> query;      
      while (await (query = Get(condition, order)).CountAsync() > 0) {
        int itemId = 0;
        MatrizH next = await query.FirstAsync();

        Vetor vector = new Vetor() { 
            EmpresaId = next.EmpresaId, DiaId = next.DiaId, 
            Inicio = next.Inicio, PInicioId = next.PInicioId, 
            Termino = next.Termino, PTerminoId = next.PTerminoId
        };
        using (VetorService service = new VetorService()) {
          await service.Insert(vector);

          using Services<VetorH> serviceh = new Services<VetorH>();
          await serviceh.Insert(new VetorH() {
                    Item = ++itemId, VetorId = vector.Id, HorarioId = next.Id
                });
        }
        int runtime = CustomCalendar.Runtime(vector.Inicio, vector.Termino);
        TimeSpan toStart = vector.Termino.Add(TimeSpan.FromMinutes(15));
        if (toStart.Days > vector.Termino.Days) {
          toStart = vector.Termino;
        }

        while (await (query = Get(Predicate.And(condition, 
                                                q => (q.DiaId == next.DiaId) && (q.Inicio >= next.Termino) &&
                                                     (q.Inicio <= toStart) && (q.PInicioId == next.PTerminoId)), order)).CountAsync() > 0) {
          next = await query.FirstAsync();
          using Services<VetorH> serviceh = new Services<VetorH>();
          await serviceh.Insert(new VetorH {
              Item = ++itemId, VetorId = vector.Id, HorarioId = next.Id
          });

          vector.Termino = next.Termino;
          vector.PTerminoId = next.PTerminoId;
          using (VetorService service = new VetorService()) {
            await service.Update(vector);
          }
          runtime = CustomCalendar.Runtime(vector.Inicio, vector.Termino);
          toStart = vector.Termino.Add(TimeSpan.FromMinutes(15));
          if (toStart.Days > vector.Termino.Days) {
            toStart = vector.Termino;
          }

          if ((runtime >= 220) && next.PTermino.Intercambio) {
            break;
          }
        }
      }
    }
  }
}
