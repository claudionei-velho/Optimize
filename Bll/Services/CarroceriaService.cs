using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class CarroceriaService : Services<Carroceria> {
    private readonly int userId;

    public CarroceriaService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Carroceria> Get(Expression<Func<Carroceria, bool>> filter = null, 
        Func<IQueryable<Carroceria>, IOrderedQueryable<Carroceria>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<Carroceria> query = (from c in context.Carrocerias
                                        join v in context.Veiculos on c.VeiculoId equals v.Id
                                        where companies.Contains(v.EmpresaId) && !v.Inativo
                                        orderby v.EmpresaId, c.VeiculoId
                                        select c).AsNoTracking().Include(c => c.Veiculo);
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
