using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class EmpresaService : Services<Empresa> {
    private readonly int userId;

    public EmpresaService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Empresa> Get(Expression<Func<Empresa, bool>> filter = null, 
        Func<IQueryable<Empresa>, IOrderedQueryable<Empresa>> orderBy = null) {
      try {
        int[] companies = context.Set<EUsuario>().AsNoTracking()
                              .Where(u => (u.UsuarioId == userId) && u.Ativo)
                              .Select(u => u.EmpresaId).Distinct().ToArray();

        IQueryable<Empresa> query = (from e in context.Empresas
                                     where companies.Contains(e.Id)
                                     orderby e.Fantasia
                                     select e).Include(e => e.Cidade)
                                         .Include(e => e.Pais).AsNoTracking();
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
