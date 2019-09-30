using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class PeriodoTipicoService : Services<PeriodoTipico> {
    protected override IQueryable<PeriodoTipico> Get(Expression<Func<PeriodoTipico, bool>> filter = null,
        Func<IQueryable<PeriodoTipico>, IOrderedQueryable<PeriodoTipico>> orderBy = null) {
      return base.Get(filter, orderBy).Include(p => p.Linha.Empresa).Include(p => p.EPeriodo);
    }
  }
}
