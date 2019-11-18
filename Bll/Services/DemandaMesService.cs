using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class DemandaMesService : Services<DemandaMes> {
    protected override IQueryable<DemandaMes> Get(Expression<Func<DemandaMes, bool>> filter = null, 
        Func<IQueryable<DemandaMes>, IOrderedQueryable<DemandaMes>> orderBy = null) {
      return base.Get(filter, orderBy).Include(d => d.Linha).Include(d => d.TCategoria);
    }
  }
}
