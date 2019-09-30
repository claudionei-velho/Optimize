using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class DimensionamentoService : Services<Dimensionamento> {
    private readonly int userId;

    public DimensionamentoService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Dimensionamento> Get(Expression<Func<Dimensionamento, bool>> filter = null,
        Func<IQueryable<Dimensionamento>, IOrderedQueryable<Dimensionamento>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<Dimensionamento> query = (from d in context.Dimensionamentos
                                             join l in context.Linhas on d.LinhaId equals l.Id
                                             where companies.Contains(l.EmpresaId)
                                             orderby l.EmpresaId, d.LinhaId, d.DiaId, d.PeriodoId, d.Sentido
                                             select d).AsNoTracking()
                                                 .Include(d => d.Linha).Include(d => d.PrLinha.EPeriodo);
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
