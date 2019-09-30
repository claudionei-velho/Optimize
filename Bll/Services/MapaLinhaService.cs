using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class MapaLinhaService : Services<MapaLinha> {
    private readonly int userId;

    public MapaLinhaService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<MapaLinha> Get(Expression<Func<MapaLinha, bool>> filter = null,
        Func<IQueryable<MapaLinha>, IOrderedQueryable<MapaLinha>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<MapaLinha> query = (from m in context.MapasLinha
                                       join l in context.Linhas on m.LinhaId equals l.Id
                                       where companies.Contains(l.EmpresaId)
                                       orderby l.EmpresaId, m.LinhaId, m.AtendimentoId, m.Sentido
                                       select m).AsNoTracking()
                                           .Include(m => m.Linha).Include(m => m.Atendimento);
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
