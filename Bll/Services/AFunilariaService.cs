using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class AFunilariaService : Services<AFunilaria> {
    protected override IQueryable<AFunilaria> Get(Expression<Func<AFunilaria, bool>> filter = null,
        Func<IQueryable<AFunilaria>, IOrderedQueryable<AFunilaria>> orderBy = null) {
      return base.Get(filter, orderBy).Include(f => f.EInstalacao.Instalacao.Empresa);
    }
  }
}
