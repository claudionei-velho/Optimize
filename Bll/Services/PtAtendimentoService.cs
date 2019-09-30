using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class PtAtendimentoService : Services<PtAtendimento> {
    private readonly int userId;

    public PtAtendimentoService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<PtAtendimento> Get(Expression<Func<PtAtendimento, bool>> filter = null,
        Func<IQueryable<PtAtendimento>, IOrderedQueryable<PtAtendimento>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<PtAtendimento> query = (from p in context.PtAtendimentos
                                           join a in context.Atendimentos on p.AtendimentoId equals a.Id
                                           join l in context.Linhas on a.LinhaId equals l.Id
                                           where companies.Contains(l.EmpresaId)
                                           orderby l.EmpresaId, a.LinhaId, a.Id, p.Id
                                           select p).AsNoTracking()
                                               .Include(p => p.Atendimento).Include(p => p.Ponto);
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
