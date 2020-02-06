using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class LinhaService : Services<Linha> {
    private readonly int userId;

    public LinhaService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Linha> Get(Expression<Func<Linha, bool>> filter = null,
        Func<IQueryable<Linha>, IOrderedQueryable<Linha>> orderBy = null) {
      try {
        int[] companies = context.Set<EUsuario>().AsNoTracking()
                              .Where(u => (u.UsuarioId == userId) && u.Ativo)
                              .Select(u => u.EmpresaId).Distinct().ToArray();

        IQueryable<Linha> query = (from l in context.Linhas
                                   where companies.Contains(l.EmpresaId)
                                   orderby l.EmpresaId, l.Id
                                   select l).Include(l => l.Empresa)
                                      .Include(l => l.EDominio.Dominio).Include(l => l.Operacao.OperLinha)
                                      .Include(l => l.CLinha.ClassLinha).Include(l => l.Lote).AsNoTracking();
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

    public string GetPontoInicial(int id, string ab) {
      Expression<Func<Itinerario, bool>> filter = q => (q.LinhaId == id) && q.Sentido.Equals(ab);
      using (Services<Itinerario> itinerarios = new Services<Itinerario>()) {
        if (itinerarios.Exists(filter)) {
          return itinerarios.GetFirst(filter).Percurso;
        }
      }
      return string.Empty;
    }

    public string GetPontoFinal(int id, string ab) {
      Expression<Func<Itinerario, bool>> filter = q => (q.LinhaId == id) && q.Sentido.Equals(ab);
      using (Services<Itinerario> itinerarios = new Services<Itinerario>()) {
        if (itinerarios.Exists(filter)) {
          return itinerarios.GetById(itinerarios.GetQuery(filter).Max(p => p.Id)).Percurso;
        }
      }
      return string.Empty;
    }
  }
}
