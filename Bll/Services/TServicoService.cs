using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class TServicoService : Services<TServico> {
    private readonly int userId;

    public TServicoService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<TServico> Get(Expression<Func<TServico, bool>> filter = null, 
        Func<IQueryable<TServico>, IOrderedQueryable<TServico>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<TServico> query = (from s in context.TServicos
                                      join t in context.Terminais on s.TerminalId equals t.Id
                                      where companies.Contains(t.EmpresaId)
                                      orderby t.EmpresaId, s.TerminalId, s.Id
                                      select s).AsNoTracking().Include(s => s.Terminal.Empresa);
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
