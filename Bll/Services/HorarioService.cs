using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class HorarioService : Services<Horario> {
    private readonly int userId;

    public HorarioService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Horario> Get(Expression<Func<Horario, bool>> filter = null,
        Func<IQueryable<Horario>, IOrderedQueryable<Horario>> orderBy = null) {
      try {
        int[] companies = context.Set<EUsuario>().AsNoTracking()
                              .Where(u => (u.UsuarioId == userId) && u.Ativo)
                              .Select(u => u.EmpresaId).Distinct().ToArray();

        IQueryable<Horario> query = (from h in context.Horarios
                                     join l in context.Linhas on h.LinhaId equals l.Id
                                     where companies.Contains(l.EmpresaId)
                                     orderby l.EmpresaId, h.LinhaId, h.DiaId, h.Sentido, h.Inicio
                                     select h).AsNoTracking()
                                         .Include(h => h.Linha.Empresa).Include(h => h.Atendimento)
                                         .Include(h => h.PrLinha.EPeriodo);
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
