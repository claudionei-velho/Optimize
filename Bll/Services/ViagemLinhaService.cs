using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class ViagemLinhaService : Services<ViagemLinha> {
    private readonly int userId;

    public ViagemLinhaService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<ViagemLinha> Get(Expression<Func<ViagemLinha, bool>> filter = null,
        Func<IQueryable<ViagemLinha>, IOrderedQueryable<ViagemLinha>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<ViagemLinha> query = (from v in context.ViagensLinha
                                         where companies.Contains(v.EmpresaId)
                                         orderby v.EmpresaId, v.LinhaId, v.AtendimentoId, v.DiaId
                                         select v).AsNoTracking()
                                             .Include(v => v.Linha).Include(v => v.Atendimento);
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
