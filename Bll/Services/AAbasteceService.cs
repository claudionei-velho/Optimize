using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class AAbasteceService : Services<AAbastece> {
    protected override IQueryable<AAbastece> Get(Expression<Func<AAbastece, bool>> filter = null, 
        Func<IQueryable<AAbastece>, IOrderedQueryable<AAbastece>> orderBy = null) {
      return base.Get(filter, orderBy).Include(a => a.EInstalacao.Instalacao.Empresa);
    }
  }
}
