using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class EDominioService : Services<EDominio> {
    private readonly int userId;

    public EDominioService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<EDominio> Get(Expression<Func<EDominio, bool>> filter = null, 
        Func<IQueryable<EDominio>, IOrderedQueryable<EDominio>> orderBy = null) {
      try {
        int[] companies = context.Set<EUsuario>().AsNoTracking()
                              .Where(u => (u.UsuarioId == userId) && u.Ativo)
                              .Select(u => u.EmpresaId).Distinct().ToArray();

        IQueryable<EDominio> query = (from d in context.EDominios
                                      where companies.Contains(d.EmpresaId)
                                      orderby d.EmpresaId, d.Id
                                      select d).AsNoTracking()
                                          .Include(d => d.Empresa).Include(d => d.Dominio);
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
