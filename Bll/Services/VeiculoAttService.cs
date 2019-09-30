using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class VeiculoAttService : Services<VeiculoAtt> {
    protected override IQueryable<VeiculoAtt> Get(Expression<Func<VeiculoAtt, bool>> filter = null, 
        Func<IQueryable<VeiculoAtt>, IOrderedQueryable<VeiculoAtt>> orderBy = null) {
      return base.Get(filter, orderBy).Include(v => v.CVeiculo).Include(v => v.CVeiculoAtt);
    }
  }
}
