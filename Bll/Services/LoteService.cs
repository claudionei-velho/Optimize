using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class LoteService : Services<Lote> {
    protected override IQueryable<Lote> Get(Expression<Func<Lote, bool>> filter = null, 
        Func<IQueryable<Lote>, IOrderedQueryable<Lote>> orderBy = null) {
      return base.Get(filter, orderBy).Include(l => l.Bacia.Municipio).AsNoTracking();
    }
  }
}
