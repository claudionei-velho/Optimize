using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class CustoService : Services<Custo> {
    private readonly int userId;

    public CustoService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Custo> Get(Expression<Func<Custo, bool>> filter = null, 
        Func<IQueryable<Custo>, IOrderedQueryable<Custo>> orderBy = null) {
      try {
        int[] companies = context.Set<EUsuario>().AsNoTracking()
                              .Where(u => (u.UsuarioId == userId) && u.Ativo)
                              .Select(u => u.EmpresaId).Distinct().ToArray();

        IQueryable<Custo> query = (from c in context.Custos
                                   where companies.Contains(c.EmpresaId)
                                   orderby c.EmpresaId, c.Referencia descending
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
