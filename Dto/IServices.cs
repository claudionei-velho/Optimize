using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dto {
  public interface IServices<TEntity> where TEntity : class {
    IQueryable<TEntity> GetQuery();
    IQueryable<TEntity> GetQuery(int skip, int take);
    IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null);

    IEnumerable<TEntity> GetAll();
    IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

    IEnumerable<dynamic> GetSelect(Expression<Func<TEntity, dynamic>> columns);
    IEnumerable<dynamic> GetSelect(Expression<Func<TEntity, dynamic>> columns,
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
    Task<IEnumerable<dynamic>> GetSelectAsync(Expression<Func<TEntity, dynamic>> columns);
    Task<IEnumerable<dynamic>> GetSelectAsync(Expression<Func<TEntity, dynamic>> columns,
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

    TEntity GetById(int id);
    TEntity GetById(object id);
    Task<TEntity> GetByIdAsync(int id);    
    Task<TEntity> GetByIdAsync(object id);
    TEntity GetFirst(Expression<Func<TEntity, bool>> filter = null);
    Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter = null);
    int GetCount(Expression<Func<TEntity, bool>> filter = null);
    Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter = null);
    bool Exists(Expression<Func<TEntity, bool>> condition);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> condition);

    Task Insert(TEntity obj);
    Task Update(TEntity obj);
    Task Delete(TEntity obj);
    Task ClearAll(TEntity obj);
  }
}
