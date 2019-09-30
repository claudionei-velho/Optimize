using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class EPeriodoService : Services<EPeriodo> {
    private readonly int userId;

    public EPeriodoService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<EPeriodo> Get(Expression<Func<EPeriodo, bool>> filter = null, 
        Func<IQueryable<EPeriodo>, IOrderedQueryable<EPeriodo>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<EPeriodo> query = (from p in context.EPeriodos
                                      where companies.Contains(p.EmpresaId)
                                      orderby p.EmpresaId, p.Id
                                      select p).AsNoTracking()
                                          .Include(p => p.Empresa).Include(p => p.Periodo);
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
