using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class CLinhaService : Services<CLinha> {
    private readonly int userId;

    public CLinhaService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<CLinha> Get(Expression<Func<CLinha, bool>> filter = null, 
        Func<IQueryable<CLinha>, IOrderedQueryable<CLinha>> orderBy = null) {
      try {
        int[] companies = context.Set<EUsuario>().AsNoTracking()
                              .Where(u => (u.UsuarioId == userId) && u.Ativo)
                              .Select(u => u.EmpresaId).Distinct().ToArray();

        IQueryable<CLinha> query = (from c in context.CLinhas
                                    where companies.Contains(c.EmpresaId)
                                    orderby c.EmpresaId, c.Id
                                    select c).AsNoTracking()
                                        .Include(c => c.Empresa).Include(c => c.ClassLinha);
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
