using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class AEstacionaService : Services<AEstaciona> {
    protected override IQueryable<AEstaciona> Get(Expression<Func<AEstaciona, bool>> filter = null, 
        Func<IQueryable<AEstaciona>, IOrderedQueryable<AEstaciona>> orderBy = null) {
      return base.Get(filter, orderBy).Include(e => e.EInstalacao.Instalacao.Empresa);
    }
  }
}
