using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class ItinerarioService : Services<Itinerario> {
    private readonly int userId;

    public ItinerarioService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Itinerario> Get(Expression<Func<Itinerario, bool>> filter = null, 
        Func<IQueryable<Itinerario>, IOrderedQueryable<Itinerario>> orderBy = null) {
      try {
        int[] companies = context.Set<EUsuario>().AsNoTracking()
                              .Where(u => (u.UsuarioId == userId) && u.Ativo)
                              .Select(u => u.EmpresaId).Distinct().ToArray();

        IQueryable<Itinerario> query = (from i in context.Itinerarios
                                        join l in context.Linhas on i.LinhaId equals l.Id
                                        where companies.Contains(l.EmpresaId)
                                        orderby l.EmpresaId, i.LinhaId, i.Id
                                        select i).AsNoTracking()
                                            .Include(i => i.Linha);
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
