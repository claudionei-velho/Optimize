using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class AdjacenciaService : Services<Adjacencia> {
    protected override IQueryable<Adjacencia> Get(Expression<Func<Adjacencia, bool>> filter = null, 
        Func<IQueryable<Adjacencia>, IOrderedQueryable<Adjacencia>> orderBy = null) {
      return base.Get(filter, orderBy).Include(a => a.Empresa)
                 .Include(a => a.Ponto).Include(a => a.Adjacente);
    }
  }
}
