using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class PtTroncoService : Services<PtTronco> {
    private readonly int userId;

    public PtTroncoService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<PtTronco> Get(Expression<Func<PtTronco, bool>> filter = null, 
      Func<IQueryable<PtTronco>, IOrderedQueryable<PtTronco>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<PtTronco> query = (from p in context.PtTroncos
                                      join t in context.Troncos on p.TroncoId equals t.Id
                                      where companies.Contains(t.EmpresaId)
                                      orderby t.EmpresaId, p.TroncoId, p.Id
                                      select p).AsNoTracking()
                                          .Include(p => p.Tronco).Include(p => p.Ponto);
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
