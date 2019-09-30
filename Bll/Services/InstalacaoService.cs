using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class InstalacaoService : Services<Instalacao> {
    private readonly int userId;

    public InstalacaoService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Instalacao> Get(Expression<Func<Instalacao, bool>> filter = null, 
        Func<IQueryable<Instalacao>, IOrderedQueryable<Instalacao>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<Instalacao> query = (from i in context.Instalacoes
                                        where companies.Contains(i.EmpresaId)
                                        orderby i.EmpresaId, i.Id
                                        select i).AsNoTracking().Include(i => i.Empresa);
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
