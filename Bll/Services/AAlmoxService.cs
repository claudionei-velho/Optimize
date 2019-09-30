using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class AAlmoxService : Services<AAlmox> {
    protected override IQueryable<AAlmox> Get(Expression<Func<AAlmox, bool>> filter = null,
        Func<IQueryable<AAlmox>, IOrderedQueryable<AAlmox>> orderBy = null) {
      return base.Get(filter, orderBy).Include(a => a.EInstalacao.Instalacao.Empresa);
    }
  }
}
