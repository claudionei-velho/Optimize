using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class TerminalService : Services<Terminal> {
    private readonly int userId;

    public TerminalService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Terminal> Get(Expression<Func<Terminal, bool>> filter = null,
        Func<IQueryable<Terminal>, IOrderedQueryable<Terminal>> orderBy = null) {
      try {
        int[] companies = context.Set<EUsuario>().AsNoTracking()
                              .Where(u => (u.UsuarioId == userId) && u.Ativo)
                              .Select(u => u.EmpresaId).Distinct().ToArray();

        IQueryable<Terminal> query = (from t in context.Terminais
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
