using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class AMantemService : Services<AMantem> {
    protected override IQueryable<AMantem> Get(Expression<Func<AMantem, bool>> filter = null,
        Func<IQueryable<AMantem>, IOrderedQueryable<AMantem>> orderBy = null) {
      return base.Get(filter, orderBy).Include(m => m.EInstalacao.Instalacao.Empresa);
    }
  }
}
