using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class PtLinhaService : Services<PtLinha> {
    private readonly int userId;

    public PtLinhaService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<PtLinha> Get(Expression<Func<PtLinha, bool>> filter = null, 
        Func<IQueryable<PtLinha>, IOrderedQueryable<PtLinha>> orderBy = null) {
      try {
        int[] companies = context.Set<EUsuario>().AsNoTracking()
                              .Where(u => (u.UsuarioId == userId) && u.Ativo)
                              .Select(u => u.EmpresaId).Distinct().ToArray();

        IQueryable<PtLinha> query = (from p in context.PtLinhas
                                     join l in context.Linhas on p.LinhaId equals l.Id
                                     where companies.Contains(l.EmpresaId)
                                     orderby l.EmpresaId, p.LinhaId, p.Id
                                     select p).AsNoTracking()
                                         .Include(p => p.Linha).Include(p => p.Ponto)
                                         .Include(p => p.Origem.Ponto).Include(p => p.Destino.Ponto);
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
