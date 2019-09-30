using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class ATrafegoService : Services<ATrafego> {
    protected override IQueryable<ATrafego> Get(Expression<Func<ATrafego, bool>> filter = null,
        Func<IQueryable<ATrafego>, IOrderedQueryable<ATrafego>> orderBy = null) {
      return base.Get(filter, orderBy).Include(t => t.EInstalacao.Instalacao.Empresa);
    }
  }
}
