using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Dto.Models;

namespace Bll.Services {
  public class TecnicalService : Services<Tecnical> {
    private readonly int userId;

    public TecnicalService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Tecnical> Get(Expression<Func<Tecnical, bool>> filter = null,
        Func<IQueryable<Tecnical>, IOrderedQueryable<Tecnical>> orderBy = null) {
      try {
        return base.Get(filter, orderBy).Include(l => l.Empresa)
                   .Include(l => l.EDominio.Dominio).Include(l => l.Operacao.OperLinha)
                   .Include(l => l.CLinha.ClassLinha).Include(l => l.Lote).AsNoTracking();
      }
      catch (Exception ex) {
        throw new Exception(ex.Message);
      }
    }
  }
}
