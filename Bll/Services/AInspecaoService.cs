using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class AInspecaoService : Services<AInspecao> {
    protected override IQueryable<AInspecao> Get(Expression<Func<AInspecao, bool>> filter = null,
        Func<IQueryable<AInspecao>, IOrderedQueryable<AInspecao>> orderBy = null) {
      return base.Get(filter, orderBy).Include(i => i.EInstalacao.Instalacao.Empresa);
    }
  }
}
