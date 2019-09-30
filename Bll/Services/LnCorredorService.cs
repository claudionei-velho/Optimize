using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class LnCorredorService : Services<LnCorredor> {
    private readonly int userId;

    public LnCorredorService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<LnCorredor> Get(Expression<Func<LnCorredor, bool>> filter = null, 
        Func<IQueryable<LnCorredor>, IOrderedQueryable<LnCorredor>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<LnCorredor> query = (from c in context.LnCorredores
                                        join l in context.Linhas on c.LinhaId equals l.Id
                                        where companies.Contains(l.EmpresaId)
                                        orderby l.EmpresaId, c.CorredorId, c.LinhaId
                                        select c).AsNoTracking()
                                            .Include(c => c.Corredor).Include(c => c.Linha);
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
