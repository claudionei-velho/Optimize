using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class CustoCoService : Services<CustoCo> {
    protected override IQueryable<CustoCo> Get(Expression<Func<CustoCo, bool>> filter = null, 
        Func<IQueryable<CustoCo>, IOrderedQueryable<CustoCo>> orderBy = null) {
      return base.Get(filter, orderBy).Include(co => co.Rubrica);
    }
  }
}
