using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class CorredorService : Services<Corredor> {
    private readonly int userId;

    public CorredorService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Corredor> Get(Expression<Func<Corredor, bool>> filter = null, 
        Func<IQueryable<Corredor>, IOrderedQueryable<Corredor>> orderBy = null) {
      try {
        int[] companies = context.Set<EUsuario>().AsNoTracking()
                              .Where(u => (u.UsuarioId == userId) && u.Ativo)
                              .Select(u => u.EmpresaId).Distinct().ToArray();

        IQueryable<Corredor> query = (from c in context.Corredores
                                      where companies.Contains(c.EmpresaId)
                                      orderby c.EmpresaId, c.Id
                                      select c).AsNoTracking().Include(c => c.Empresa);
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
