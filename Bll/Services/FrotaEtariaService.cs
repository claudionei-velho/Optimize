using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class FrotaEtariaService : Services<FrotaEtaria> {
    private readonly int userId;

    public FrotaEtariaService(int? _userId = null) {
      userId = _userId ?? 1;
    }

    protected override IQueryable<FrotaEtaria> Get(Expression<Func<FrotaEtaria, bool>> filter = null,
        Func<IQueryable<FrotaEtaria>, IOrderedQueryable<FrotaEtaria>> orderBy = null) {
      try {
        int[] companies = context.Set<EUsuario>().AsNoTracking()
                              .Where(u => (u.UsuarioId == userId) && u.Ativo)
                              .Select(u => u.EmpresaId).Distinct().ToArray();

        IQueryable<FrotaEtaria> query = (from f in context.FrotaEtarias
                                         where companies.Contains(f.EmpresaId)
                                         orderby f.EmpresaId, f.EtariaId
                                         select f).AsNoTracking()
                                             .Include(e => e.Empresa).Include(e => e.FxEtaria);
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

    public decimal? MediaIdade(Expression<Func<FrotaEtaria, bool>> filter = null) {
      try {
        return (decimal)Get(filter).Sum(q => q.EqvIdade) / Get(filter).Sum(q => q.Frota);
      }
      catch (DivideByZeroException) {
        return null;
      }
    }
  }
}
