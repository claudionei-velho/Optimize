using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Dto.Models;

namespace Bll.Services {
  public class VeiculoService : Services<Veiculo> {
    private readonly int userId;

    public VeiculoService(int? _userId = null) {
      this.userId = _userId ?? 1;
    }

    protected override IQueryable<Veiculo> Get(Expression<Func<Veiculo, bool>> filter = null,
        Func<IQueryable<Veiculo>, IOrderedQueryable<Veiculo>> orderBy = null) {
      try {
        int[] companies = (from u in context.EUsuarios
                           where u.UsuarioId == userId && u.Ativo
                           select u.EmpresaId).Distinct().ToArray();

        IQueryable<Veiculo> query = (from v in context.Veiculos
                                     where companies.Contains(v.EmpresaId) && !v.Inativo
                                     orderby v.EmpresaId, v.Id
                                     select v).AsNoTracking()
                                         .Include(v => v.Empresa).Include(v => v.CVeiculo);
        if (filter != null) {
          query = query.Where(filter);
        }
        if (orderBy != null) {
          query = orderBy(query);
        }
        return query;
      }
      catch (Exception ex) {
        throw new Exception(ex.Message);
      }
    }

    public IEnumerable<dynamic> AddCarrocerias(Expression<Func<Veiculo, dynamic>> columns) {
      try {
        int[] usedId = (from c in context.Carrocerias
                        select c.VeiculoId).ToArray();

        IQueryable<Veiculo> query = (from v in Get()
                                     where !usedId.Contains(v.Id)
                                     select v).AsNoTracking();
        return query.Select(columns).ToList();
      }
      catch (Exception ex) {
        throw new Exception(ex.Message);
      }
    }

    public async Task<IEnumerable<dynamic>> AddCarroceriasAsync(Expression<Func<Veiculo, dynamic>> columns) {
      try {
        int[] usedId = (from c in context.Carrocerias
                        select c.VeiculoId).ToArray();

        IQueryable<Veiculo> query = (from v in Get()
                                     where !usedId.Contains(v.Id)
                                     select v).AsNoTracking();
        return await query.Select(columns).ToListAsync();
      }
      catch (Exception ex) {
        throw new Exception(ex.Message);
      }
    }

    public IEnumerable<dynamic> AddChassis(Expression<Func<Veiculo, dynamic>> columns) {
      try {
        int[] usedId = (from c in context.Chassis
                        select c.VeiculoId).ToArray();

        IQueryable<Veiculo> query = (from v in Get()
                                     where !usedId.Contains(v.Id)
                                     select v).AsNoTracking();
        return query.Select(columns).ToList();
      }
      catch (Exception ex) {
        throw new Exception(ex.Message);
      }
    }

    public async Task<IEnumerable<dynamic>> AddChassisAsync(Expression<Func<Veiculo, dynamic>> columns) {
      try {
        int[] usedId = (from c in context.Chassis
                        select c.VeiculoId).ToArray();

        IQueryable<Veiculo> query = (from v in Get()
                                     where !usedId.Contains(v.Id)
                                     select v).AsNoTracking();
        return await query.Select(columns).ToListAsync();
      }
      catch (Exception ex) {
        throw new Exception(ex.Message);
      }
    }
  }
}
