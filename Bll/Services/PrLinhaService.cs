using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class PrLinhaService : Services<PrLinha> {
    private readonly int userId;

    public PrLinhaService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<PrLinha> Get(Expression<Func<PrLinha, bool>> filter = null, 
        Func<IQueryable<PrLinha>, IOrderedQueryable<PrLinha>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<PrLinha> query = (from p in context.PrLinhas
                                     join l in context.Linhas on p.LinhaId equals l.Id
                                     where companies.Contains(l.EmpresaId)
                                     orderby l.EmpresaId, p.LinhaId, p.Id
                                     select p).AsNoTracking()
                                         .Include(p => p.Linha.Empresa).Include(p => p.EPeriodo)
                                         .Include(p => p.CVeiculo).Include(p => p.Ocupacao);
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
