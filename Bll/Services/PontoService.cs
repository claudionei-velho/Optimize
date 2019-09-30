using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class PontoService : Services<Ponto> {
    private readonly int userId;

    public PontoService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Ponto> Get(Expression<Func<Ponto, bool>> filter = null, 
        Func<IQueryable<Ponto>, IOrderedQueryable<Ponto>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<Ponto> query = (from p in context.Pontos
                                   where companies.Contains(p.EmpresaId)
                                   orderby p.EmpresaId, p.Id
                                   select p).AsNoTracking().Include(p => p.Empresa);
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
