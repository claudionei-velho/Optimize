using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class ChassiService : Services<Chassi> {
    private readonly int userId;

    public ChassiService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Chassi> Get(Expression<Func<Chassi, bool>> filter = null,
        Func<IQueryable<Chassi>, IOrderedQueryable<Chassi>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<Chassi> query = (from c in context.Chassis
                                    join v in context.Veiculos on c.VeiculoId equals v.Id
                                    where companies.Contains(v.EmpresaId) && !v.Inativo
                                    orderby v.EmpresaId, c.VeiculoId
                                    select c).AsNoTracking()
                                        .Include(c => c.Veiculo).Include(c => c.Motor);
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
