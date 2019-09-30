using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class LnTerminalService : Services<LnTerminal> {
    private readonly int userId;

    public LnTerminalService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<LnTerminal> Get(Expression<Func<LnTerminal, bool>> filter = null, 
        Func<IQueryable<LnTerminal>, IOrderedQueryable<LnTerminal>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<LnTerminal> query = (from t in context.LnTerminais
                                        join l in context.Linhas on t.LinhaId equals l.Id
                                        where companies.Contains(l.EmpresaId)
                                        orderby l.EmpresaId, t.TerminalId, t.LinhaId
                                        select t).AsNoTracking()
                                            .Include(c => c.Terminal).Include(c => c.Linha);
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
