using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class TotalViagemService : Services<TotalViagem> {
    private readonly int userId;

    public TotalViagemService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<TotalViagem> Get(Expression<Func<TotalViagem, bool>> filter = null, 
        Func<IQueryable<TotalViagem>, IOrderedQueryable<TotalViagem>> orderBy = null) {
      try {
        int[] companies = context.Set<EUsuario>().AsNoTracking()
                              .Where(u => (u.UsuarioId == userId) && u.Ativo)
                              .Select(u => u.EmpresaId).Distinct().ToArray();

        IQueryable<TotalViagem> query = (from t in context.TotalViagens
                                         join l in context.Linhas on t.LinhaId equals l.Id
                                         where companies.Contains(l.EmpresaId)
                                         orderby l.EmpresaId, t.LinhaId, t.DiaId, t.PeriodoId, t.Sentido
                                         select t).AsNoTracking()
                                             .Include(t => t.Linha.Empresa).Include(t => t.PrLinha.EPeriodo);
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
