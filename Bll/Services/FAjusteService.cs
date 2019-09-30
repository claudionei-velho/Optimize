using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class FAjusteService : Services<FAjuste> {
    private readonly int userId;

    public FAjusteService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<FAjuste> Get(Expression<Func<FAjuste, bool>> filter = null, 
        Func<IQueryable<FAjuste>, IOrderedQueryable<FAjuste>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<FAjuste> query = (from f in context.FAjustes
                                     join l in context.Linhas on f.LinhaId equals l.Id
                                     where companies.Contains(l.EmpresaId)
                                     orderby l.EmpresaId, f.LinhaId, f.Ano, f.Mes
                                     select f).AsNoTracking().Include(f => f.Linha.Empresa);
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
