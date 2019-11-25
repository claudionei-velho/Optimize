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
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<Linha> query = (from l in context.Linhas
                                   where companies.Contains(l.EmpresaId)
                                   orderby l.EmpresaId, l.Id
                                   select l).AsNoTracking()
                                      .Include(l => l.Empresa).Include(l => l.EDominio.Dominio)
                                      .Include(l => l.Operacao.OperLinha).Include(l => l.CLinha.ClassLinha);
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
      using Services<Itinerario> itinerarios = new Services<Itinerario>();
      Expression<Func<Itinerario, bool>> where = q => (q.LinhaId == id) && q.Sentido.Equals(ab);

      string result = string.Empty;
      if (itinerarios.GetQuery(where).Count() > 0) {
        result = itinerarios.GetFirst(where).Percurso;
      }
      return result;
    }

    public string GetPontoFinal(int id, string ab) {
      using Services<Itinerario> itinerarios = new Services<Itinerario>();
      Expression<Func<Itinerario, bool>> where = q => (q.LinhaId == id) && q.Sentido.Equals(ab);

      string result = string.Empty;
      if (itinerarios.GetQuery(where).Count() > 0) {
        result = itinerarios.GetById(itinerarios.GetQuery(where).Max(p => p.Id)).Percurso;
      }
      return result;
    }
  }
}
