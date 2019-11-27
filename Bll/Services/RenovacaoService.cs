using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class RenovacaoService : Services<Renovacao> {
    private readonly int userId;

    public RenovacaoService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Renovacao> Get(Expression<Func<Renovacao, bool>> filter = null, 
        Func<IQueryable<Renovacao>, IOrderedQueryable<Renovacao>> orderBy = null) {
      try {
        int[] companies = context.Set<EUsuario>().AsNoTracking()
                              .Where(u => (u.UsuarioId == userId) && u.Ativo)
                              .Select(u => u.EmpresaId).Distinct().ToArray();

        IQueryable<Renovacao> query = (from r in context.Renovacoes
                                       join l in context.Linhas on r.LinhaId equals l.Id
                                       where companies.Contains(l.EmpresaId)
                                       orderby l.EmpresaId, r.LinhaId, r.Ano, r.Mes
                                       select r).AsNoTracking().Include(r => r.Linha.Empresa);
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
