using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class FViagemService : Services<FViagem> {
    protected override IQueryable<FViagem> Get(Expression<Func<FViagem, bool>> filter = null, 
        Func<IQueryable<FViagem>, IOrderedQueryable<FViagem>> orderBy = null) {
      return base.Get(filter, orderBy).Include(f => f.Viagem.LnPesquisa.Linha).Include(f => f.PtLinha.Ponto);
    }
  }
}
