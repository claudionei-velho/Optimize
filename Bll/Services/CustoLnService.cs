using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class CustoLnService : Services<CustoLn>{
    protected override IQueryable<CustoLn> Get(Expression<Func<CustoLn, bool>> filter = null, 
        Func<IQueryable<CustoLn>, IOrderedQueryable<CustoLn>> orderBy = null) {
      return base.Get(filter, orderBy).Include(ln => ln.Rubrica);
    }
  }
}
