using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class ViagemService : Services<Viagem> {
    private readonly int userId;

    public ViagemService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Viagem> Get(Expression<Func<Viagem, bool>> filter = null, 
        Func<IQueryable<Viagem>, IOrderedQueryable<Viagem>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<Viagem> query = (from v in context.Viagens
                                    join l in context.LnPesquisas on v.LinhaId equals l.Id
                                    join p in context.Pesquisas on l.PesquisaId equals p.Id
                                    where companies.Contains(p.EmpresaId)
                                    orderby p.EmpresaId, v.LinhaId, v.Data, v.Item, v.Inicio, v.Sentido
                                    select v).AsNoTracking().Include(v => v.LnPesquisa.Linha)
                                        .Include(v => v.LnPesquisa.Pesquisa)
                                        .Include(v => v.Horario).Include(v => v.PtLinha.Ponto)
                                        .Include(v => v.Veiculo).Include(v => v.PrLinha.EPeriodo);
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
