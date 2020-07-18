using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class ReferenciaService : Services<Referencia> {
    private readonly int _userId;

    public ReferenciaService(int? userId = null) {
      _userId = userId ?? 1;
    }

    protected override IQueryable<Referencia> Get(Expression<Func<Referencia, bool>> filter = null, 
        Func<IQueryable<Referencia>, IOrderedQueryable<Referencia>> orderBy = null) {
      try {
        int[] companies = context.EUsuarios.AsNoTracking()
                              .Where(u => (u.UsuarioId == _userId) && u.Ativo)
                              .Select(u => u.EmpresaId).Distinct().ToArray();

        IQueryable<Referencia> query = (from r in context.Referencias
                                        join l in context.Linhas on r.LinhaId equals l.Id
                                        where companies.Contains(l.EmpresaId)
                                        orderby l.EmpresaId, r.LinhaId, r.Id
                                        select r).AsNoTracking()
                                            .Include(r => r.Linha).Include(r => r.Atendimento)
                                            .Include(r => r.PInicio).Include(r => r.PTermino);
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
