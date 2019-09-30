using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class OfertaService : Services<Oferta> {
    private readonly int userId;

    public OfertaService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Oferta> Get(Expression<Func<Oferta, bool>> filter = null, 
        Func<IQueryable<Oferta>, IOrderedQueryable<Oferta>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<Oferta> query = (from f in context.Ofertas
                                    join l in context.Linhas on f.LinhaId equals l.Id
                                    where companies.Contains(l.EmpresaId)
                                    orderby l.EmpresaId, f.LinhaId, f.Ano, f.Mes, f.Categoria
                                    select f).AsNoTracking()
                                        .Include(f => f.Linha.Empresa).Include(f => f.TCategoria);
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
