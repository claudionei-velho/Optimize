using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class VetorService : Services<Vetor> {
    protected override IQueryable<Vetor> Get(Expression<Func<Vetor, bool>> filter = null, 
        Func<IQueryable<Vetor>, IOrderedQueryable<Vetor>> orderBy = null) {
      return base.Get(filter, orderBy);
      //return base.Get(filter, orderBy).Include(v => v.Empresa)
      //           .Include(v => v.PInicio).Include(v => v.PTermino);
    }
  }
}
