using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class ArcoService : Services<Arco> {
    protected override IQueryable<Arco> Get(Expression<Func<Arco, bool>> filter = null, 
        Func<IQueryable<Arco>, IOrderedQueryable<Arco>> orderBy = null) {
      return base.Get(filter, orderBy).Include(v => v.PInicio).Include(v => v.PTermino);
    }
  }
}
