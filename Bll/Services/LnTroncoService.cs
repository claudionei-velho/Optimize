using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class LnTroncoService : Services<LnTronco> {
    private readonly int userId;

    public LnTroncoService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<LnTronco> Get(Expression<Func<LnTronco, bool>> filter = null, 
        Func<IQueryable<LnTronco>, IOrderedQueryable<LnTronco>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<LnTronco> query = (from l in context.LnTroncos
                                      join t in context.Troncos on l.TroncoId equals t.Id
                                      where companies.Contains(t.EmpresaId)
                                      orderby t.EmpresaId, l.TroncoId, l.LinhaId
                                      select l).AsNoTracking()
                                          .Include(t => t.Tronco).Include(t => t.Linha);
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
