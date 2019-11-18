using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class DemandaAnoService : Services<DemandaAno> {
    protected override IQueryable<DemandaAno> Get(Expression<Func<DemandaAno, bool>> filter = null, 
        Func<IQueryable<DemandaAno>, IOrderedQueryable<DemandaAno>> orderBy = null) {
      return base.Get(filter, orderBy).Include(d => d.Linha).Include(d => d.TCategoria);
    }
  }
}
