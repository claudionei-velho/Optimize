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
        int[] companies = context.Set<EUsuario>().AsNoTracking()
                              .Where(u => (u.UsuarioId == userId) && u.Ativo)
                              .Select(u => u.EmpresaId).Distinct().ToArray();

        IQueryable<Dimensionamento> query = (from d in context.Dimensionamentos
                                             join l in context.Linhas on d.LinhaId equals l.Id
                                             where companies.Contains(l.EmpresaId)
                                             orderby l.EmpresaId, d.LinhaId, d.DiaId, d.PeriodoId, d.Sentido
                                             select d).AsNoTracking().Include(d => d.Pesquisa)
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

    public int? TempoViagem(Expression<Func<Dimensionamento, bool>> condition) {
      var query = context.Set<Dimensionamento>().AsNoTracking()
                      .Where(condition)
                      .GroupBy(d => new { d.PeriodoId, d.Sentido })
                      .Select(f => new {
                          ab = f.Max(p => p.CicloAB),
                          ba = f.Max(p => p.CicloBA),
                          sum = f.Sum(p => p.QtdViagens) });
      try {
        return (int)Math.Ceiling(query.Sum(q => q.sum * ((q.ab ?? 0) + (q.ba ?? 0))) /
                                   (decimal)query.Sum(q => q.sum));
      }
      catch (DivideByZeroException) {
        return null;
      }
    }

    public int TempoTotal(Expression<Func<Dimensionamento, bool>> condition) {
      var query = context.Set<Dimensionamento>().AsNoTracking()
                      .Where(condition)
                      .Select(d => new { ab = d.CicloAB, 
                                         ba = d.CicloBA, 
                                         sum = d.QtdViagens });
      return query.Sum(q => q.sum * ((q.ab ?? 0) + (q.ba ?? 0)));
    }

    public decimal Extensao(Expression<Func<Dimensionamento, bool>> condition) {
      var query = context.Set<Dimensionamento>().AsNoTracking()
                      .Where(condition)
                      .Select(d => new { sum = d.QtdViagens, 
                                         km = d.Extensao });
      try {
        return query.Sum(q => q.sum * (q.km ?? 0)) / query.Sum(q => q.sum);
      }
      catch (DivideByZeroException) {
        return (decimal)query.Max(q => q.km);
      }
    }
  }
}
