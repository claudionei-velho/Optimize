using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class SpecVeiculoService : Services<SpecVeiculo> {
    private readonly int userId;

    public SpecVeiculoService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<SpecVeiculo> Get(Expression<Func<SpecVeiculo, bool>> filter = null,
        Func<IQueryable<SpecVeiculo>, IOrderedQueryable<SpecVeiculo>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<SpecVeiculo> query = (from v in context.SpecVeiculos
                                         where companies.Contains(v.EmpresaId)
                                         orderby v.EmpresaId, v.Id
                                         select v).AsNoTracking()
                                             .Include(v => v.Empresa).Include(v => v.CVeiculo)
                                             .Include(v => v.Motor).Include(v => v.FxEtaria);
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
