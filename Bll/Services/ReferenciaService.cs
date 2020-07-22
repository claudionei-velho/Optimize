using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class ReferenciaService : Services<Referencia> {
    protected override IQueryable<Referencia> Get(Expression<Func<Referencia, bool>> filter = null, 
      Func<IQueryable<Referencia>, IOrderedQueryable<Referencia>> orderBy = null) {
      return base.Get(filter, orderBy)
                 .Include(r => r.Linha).Include(r => r.Atendimento)
                 .Include(r => r.PInicio).Include(r => r.PTermino);
    }
  }
}
