using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class OperacaoService : Services<Operacao> {
    private readonly int userId;

    public OperacaoService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Operacao> Get(Expression<Func<Operacao, bool>> filter = null, 
        Func<IQueryable<Operacao>, IOrderedQueryable<Operacao>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<Operacao> query = (from o in context.Operacoes
                                      where companies.Contains(o.EmpresaId)
                                      orderby o.EmpresaId, o.Id
                                      select o).AsNoTracking()
                                          .Include(o => o.Empresa).Include(o => o.OperLinha);
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
