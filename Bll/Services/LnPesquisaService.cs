using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class LnPesquisaService : Services<LnPesquisa> {
    private readonly int userId;

    public LnPesquisaService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<LnPesquisa> Get(Expression<Func<LnPesquisa, bool>> filter = null, 
        Func<IQueryable<LnPesquisa>, IOrderedQueryable<LnPesquisa>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<LnPesquisa> query = (from l in context.LnPesquisas
                                        join p in context.Pesquisas on l.PesquisaId equals p.Id
                                        where companies.Contains(p.EmpresaId)
                                        orderby p.EmpresaId, l.LinhaId
                                        select l).AsNoTracking()
                                            .Include(l => l.Pesquisa.Empresa).Include(l => l.Linha);
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
