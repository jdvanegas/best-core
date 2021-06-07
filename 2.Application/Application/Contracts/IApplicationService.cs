using Domain.Common;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Contracts
{
  public interface IApplicationService<TEntity> : IDisposable where TEntity : class
  {
    Task<Result<TEntity>> Create(TEntity entity);

    Task<Result<TEntity>> Get(Expression<Func<TEntity, bool>> predicate);

    IQueryable<TEntity> Queryable(bool @readonly = true);

    IQueryable<TEntity> Queryable(Expression<Func<TEntity, bool>> predicate, bool @readonly = true);

    Task<Result<bool>> Remove(TEntity entity);

    Task<Result<long>> Remove(Expression<Func<TEntity, bool>> predicate);

    Task<Result<TEntity>> Update(TEntity entity);
  }
}