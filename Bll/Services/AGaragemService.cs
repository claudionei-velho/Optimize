using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class AGaragemService : Services<AGaragem> {
    protected override IQueryable<AGaragem> Get(Expression<Func<AGaragem, bool>> filter = null,
        Func<IQueryable<AGaragem>, IOrderedQueryable<AGaragem>> orderBy = null) {
      return base.Get(filter, orderBy).Include(g => g.EInstalacao.Instalacao.Empresa);
    }
  }
}
