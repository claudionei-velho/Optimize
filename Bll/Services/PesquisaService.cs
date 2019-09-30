using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class PesquisaService : Services<Pesquisa> {
    private readonly int userId;

    public PesquisaService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Pesquisa> Get(Expression<Func<Pesquisa, bool>> filter = null, 
        Func<IQueryable<Pesquisa>, IOrderedQueryable<Pesquisa>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<Pesquisa> query = (from p in context.Pesquisas
                                      where companies.Contains(p.EmpresaId)
                                      orderby p.EmpresaId, p.Id
                                      select p).AsNoTracking()
                                          .Include(p => p.Empresa).Include(p => p.Terminal)
                                          .Include(p => p.Tronco).Include(p => p.Corredor)
                                          .Include(p => p.Operacao.OperLinha)
                                          .Include(p => p.CLinha.ClassLinha);
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
