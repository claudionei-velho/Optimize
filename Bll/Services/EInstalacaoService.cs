using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class EInstalacaoService : Services<EInstalacao> {
    private readonly int userId;

    public EInstalacaoService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<EInstalacao> Get(Expression<Func<EInstalacao, bool>> filter = null,
        Func<IQueryable<EInstalacao>, IOrderedQueryable<EInstalacao>> orderBy = null) {
      try {
        int[] companies = context.Set<EUsuario>().AsNoTracking()
                              .Where(u => (u.UsuarioId == userId) && u.Ativo)
                              .Select(u => u.EmpresaId).Distinct().ToArray();

        IQueryable<EInstalacao> query = (from i in context.EInstalacoes
                                         join g in context.Instalacoes on i.InstalacaoId equals g.Id
                                         where companies.Contains(g.EmpresaId)
                                         orderby i.InstalacaoId, i.Id
                                         select i).AsNoTracking()
                                             .Include(i => i.Instalacao.Empresa).Include(i => i.FInstalacao);
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

    public decimal? TotalArea(Expression<Func<EInstalacao, bool>> filter = null) {
      return Get(filter).Sum(q => q.AreaTotal);
    }

    public decimal? TotalAreaCoberta(Expression<Func<EInstalacao, bool>> filter = null) {
      return Get(filter).Sum(q => q.AreaCoberta);
    }

    public int? TotalEmpregados(Expression<Func<EInstalacao, bool>> filter = null) {
      return Get(filter).Sum(q => q.QtdEmpregados);
    }
  }
}
