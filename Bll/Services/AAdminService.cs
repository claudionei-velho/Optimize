using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class AAdminService : Services<AAdmin> {
    protected override IQueryable<AAdmin> Get(Expression<Func<AAdmin, bool>> filter = null,
        Func<IQueryable<AAdmin>, IOrderedQueryable<AAdmin>> orderBy = null) {
      return base.Get(filter, orderBy).Include(a => a.EInstalacao.Instalacao.Empresa);
    }
  }
}
