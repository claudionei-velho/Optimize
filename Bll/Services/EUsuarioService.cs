using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class EUsuarioService : Services<EUsuario> {
    protected override IQueryable<EUsuario> Get(Expression<Func<EUsuario, bool>> filter = null,
        Func<IQueryable<EUsuario>, IOrderedQueryable<EUsuario>> orderBy = null) {
      return base.Get(filter, orderBy).Include(e => e.Empresa).Include(e => e.Usuario);
    }
  }
}
