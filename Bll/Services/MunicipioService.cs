using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class MunicipioService : Services<Municipio> {
    protected override IQueryable<Municipio> Get(Expression<Func<Municipio, bool>> filter = null, 
        Func<IQueryable<Municipio>, IOrderedQueryable<Municipio>> orderBy = null) {
      return base.Get(filter, orderBy).Include(m => m.Uf);
    }
  }
}
