using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class TroncoService : Services<Tronco> {
    private readonly int userId;

    public TroncoService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Tronco> Get(Expression<Func<Tronco, bool>> filter = null, 
        Func<IQueryable<Tronco>, IOrderedQueryable<Tronco>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<Tronco> query = (from t in context.Troncos
                                    where companies.Contains(t.EmpresaId)
                                    orderby t.EmpresaId, t.Id
                                    select t).AsNoTracking().Include(t => t.Empresa);
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
