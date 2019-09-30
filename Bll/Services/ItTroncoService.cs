using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class ItTroncoService : Services<ItTronco> {
    private readonly int userId;

    public ItTroncoService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<ItTronco> Get(Expression<Func<ItTronco, bool>> filter = null, 
        Func<IQueryable<ItTronco>, IOrderedQueryable<ItTronco>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<ItTronco> query = (from i in context.ItTroncos
                                      join t in context.Troncos on i.TroncoId equals t.Id
                                      where companies.Contains(t.EmpresaId)
                                      orderby t.EmpresaId, i.TroncoId, i.Sentido, i.Id
                                      select i).AsNoTracking()
                                          .Include(i => i.Tronco).Include(i => i.Via);
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
  }
}
