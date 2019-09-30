using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class TCategoriaService : Services<TCategoria> {
    private readonly int userId;

    public TCategoriaService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<TCategoria> Get(Expression<Func<TCategoria, bool>> filter = null, 
        Func<IQueryable<TCategoria>, IOrderedQueryable<TCategoria>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<TCategoria> query = (from c in context.TCategorias
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
