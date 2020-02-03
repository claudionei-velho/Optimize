using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Dal;
using Dto;

namespace Bll {
  public class Services<TEntity> : IDisposable, IServices<TEntity> where TEntity : class {
    protected DataContext context = new DataContext();

    protected virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null) {
      try {
        IQueryable<TEntity> query = context.Set<TEntity>().AsNoTracking();
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

    public virtual IQueryable<TEntity> GetQuery() {
      return Get();
    }

    public virtual IQueryable<TEntity> GetQuery(int skip, int take) {
      return Get().Skip(skip).Take(take);
    }

    public virtual IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null) {
      IQueryable<TEntity> query = Get(filter, orderBy);
      if (skip.HasValue) {
        query = query.Skip(skip.Value);
      }
      if (take.HasValue) {
        query = query.Take(take.Value);
      }
      return query;
    }

    public virtual IEnumerable<TEntity> GetAll() {
      return Get().ToList();
    }

    public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null) {
      return Get(filter, orderBy).ToList();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync() {
      return await Get().ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null) {
      return await Get(filter, orderBy).ToListAsync();
    }

    public IEnumerable<dynamic> GetSelect(Expression<Func<TEntity, dynamic>> columns) {
      return Get().Select(columns).ToList();
    }

    public IEnumerable<dynamic> GetSelect(Expression<Func<TEntity, dynamic>> columns,
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null) {
      return Get(filter, orderBy).Select(columns).ToList();
    }

    public async Task<IEnumerable<dynamic>> GetSelectAsync(Expression<Func<TEntity, dynamic>> columns) {
      return await Get().Select(columns).ToListAsync();
    }

    public async Task<IEnumerable<dynamic>> GetSelectAsync(Expression<Func<TEntity, dynamic>> columns,
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null) {
      return await Get(filter, orderBy).Select(columns).ToListAsync();
    }

    public TEntity GetById(int id) {
      try {
        return context.Set<TEntity>().Find(id);
      }
      catch (Exception ex) {
        throw new Exception(ex.Message);
      }
    }

    public TEntity GetById(object id) {
      try {
        return context.Set<TEntity>().Find(id);
      }
      catch (Exception ex) {
        throw new Exception(ex.Message);
      }
    }

    public async Task<TEntity> GetByIdAsync(int id) {
      try {
        return await context.Set<TEntity>().FindAsync(id);
      }
      catch (Exception ex) {
        throw new Exception(ex.Message);
      }
    }

    public async Task<TEntity> GetByIdAsync(object id) {
      try {
        return await context.Set<TEntity>().FindAsync(id);
      }
      catch (Exception ex) {
        throw new Exception(ex.Message);
      }
    }

    public TEntity GetFirst(Expression<Func<TEntity, bool>> filter = null) {
      return Get().FirstOrDefault(filter ?? throw new ArgumentNullException(nameof(filter)));
    }

    public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter = null) {
      return await Get().FirstOrDefaultAsync(filter ?? throw new ArgumentNullException(nameof(filter)));
    }

    public int GetCount(Expression<Func<TEntity, bool>> filter = null) {
      return Get(filter).Count();
    }

    public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null) {
      return await Get(filter).CountAsync();
    }

    public async Task Insert(TEntity obj) {
      using (context) {
        try {
          context.Set<TEntity>().Add(obj);
          await context.SaveChangesAsync();
        }
        catch (Exception ex) {
          throw new Exception(ex.Message);
        }
      }
    }

    public async Task Update(TEntity obj) {
      using (context) {
        try {
          context.Entry(obj).State = EntityState.Modified;
          await context.SaveChangesAsync();
        }
        catch (Exception ex) {
          throw new Exception(ex.Message);
        }
      }
    }

    public async Task Delete(TEntity obj) {
      using (context) {
        try {
          context.Set<TEntity>().Remove(obj);
          await context.SaveChangesAsync();
        }
        catch (Exception ex) {
          throw new Exception(ex.Message);
        }
      }
    }

    public async Task ClearAll(TEntity obj) {
      using (context) {
        try {
          context.Set<TEntity>().RemoveRange(context.Set<TEntity>());
          await context.SaveChangesAsync();
        }
        catch (Exception ex) {
          throw new Exception(ex.Message);
        }
      }
    }

    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
      if (disposing) {
        context?.Dispose();
      }
    }
  }
}
