using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class ECVeiculoService : Services<ECVeiculo> {
    private readonly int userId;

    public ECVeiculoService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<ECVeiculo> Get(Expression<Func<ECVeiculo, bool>> filter = null,
        Func<IQueryable<ECVeiculo>, IOrderedQueryable<ECVeiculo>> orderBy = null) {
      try {
        int[] companies = context.Set<EUsuario>().AsNoTracking()
                              .Where(u => (u.UsuarioId == userId) && u.Ativo)
                              .Select(u => u.EmpresaId).Distinct().ToArray();

        IQueryable<ECVeiculo> query = (from v in context.ECVeiculos
                                       where companies.Contains(v.EmpresaId)
                                       orderby v.EmpresaId, v.Id
                                       select v).AsNoTracking()
                                           .Include(v => v.Empresa).Include(v => v.CVeiculo);
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
