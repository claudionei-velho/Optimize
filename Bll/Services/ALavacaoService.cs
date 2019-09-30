using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class ALavacaoService : Services<ALavacao> {
    protected override IQueryable<ALavacao> Get(Expression<Func<ALavacao, bool>> filter = null,
        Func<IQueryable<ALavacao>, IOrderedQueryable<ALavacao>> orderBy = null) {
      return base.Get(filter, orderBy).Include(l => l.EInstalacao.Instalacao.Empresa);
    }
  }
}
