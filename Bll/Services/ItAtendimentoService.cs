using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class ItAtendimentoService : Services<ItAtendimento> {
    private readonly int userId;

    public ItAtendimentoService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<ItAtendimento> Get(Expression<Func<ItAtendimento, bool>> filter = null, 
        Func<IQueryable<ItAtendimento>, IOrderedQueryable<ItAtendimento>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<ItAtendimento> query = (from i in context.ItAtendimentos
                                           join a in context.Atendimentos on i.AtendimentoId equals a.Id
                                           join l in context.Linhas on a.LinhaId equals l.Id
                                           where companies.Contains(l.EmpresaId)
                                           orderby l.EmpresaId, a.LinhaId, a.Id, i.Id
                                           select i).AsNoTracking()
                                               .Include(i => i.Atendimento.Linha).Include(i => i.Via);
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
