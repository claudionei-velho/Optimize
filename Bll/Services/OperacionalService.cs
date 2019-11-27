using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class OperacionalService : Services<Operacional> {
    private readonly int userId;

    public OperacionalService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Operacional> Get(Expression<Func<Operacional, bool>> filter = null,
        Func<IQueryable<Operacional>, IOrderedQueryable<Operacional>> orderBy = null) {
      try {
        int[] companies = context.Set<EUsuario>().AsNoTracking()
                              .Where(u => (u.UsuarioId == userId) && u.Ativo)
                              .Select(u => u.EmpresaId).Distinct().ToArray();

        IQueryable<Operacional> query = (from p in context.Operacionais
                                         where companies.Contains(p.EmpresaId)
                                         orderby p.EmpresaId, p.Prefixo
                                         select p).AsNoTracking()
                                             .Include(p => p.Linha.Empresa).Include(p => p.Atendimento);
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
