using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class ALubrificaService : Services<ALubrifica> {
    protected override IQueryable<ALubrifica> Get(Expression<Func<ALubrifica, bool>> filter = null,
        Func<IQueryable<ALubrifica>, IOrderedQueryable<ALubrifica>> orderBy = null) {
      return base.Get(filter, orderBy).Include(l => l.EInstalacao.Instalacao.Empresa);
    }
  }
}
