using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class BaciaService : Services<Bacia> {
    protected override IQueryable<Bacia> Get(Expression<Func<Bacia, bool>> filter = null, 
        Func<IQueryable<Bacia>, IOrderedQueryable<Bacia>> orderBy = null) {
      return base.Get(filter, orderBy).Include(b => b.Municipio).AsNoTracking();
    }
  }
}
