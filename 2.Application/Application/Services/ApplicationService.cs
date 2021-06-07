using Application.Contracts;
using AutoMapper;
using Domain.Common;
using Domain.Repository;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Services
{
  public class ApplicationService<TEntity> : IApplicationService<TEntity> where TEntity : class
  {
    private readonly IRepository<TEntity> _repository;
    protected readonly IMapper _mapper;

    public ApplicationService(IRepository<TEntity> repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<Result<TEntity>> Create(TEntity entity)
    {
      return await _repository.Create(entity);
    }

    public void Dispose()
    {
      GC.SuppressFinalize(this);
    }

    public async Task<Result<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
    {
      return await _repository.Get(predicate);
    }

    public IQueryable<TEntity> Queryable(bool @readonly = true)
    {
      return _repository.Queryable(@readonly);
    }

    public IQueryable<TEntity> Queryable(Expression<Func<TEntity, bool>> predicate, bool @readonly = true)
    {
      return _repository.Queryable(predicate, @readonly);
    }

    public async Task<Result<bool>> Remove(TEntity entity)
    {
      return await _repository.Remove(entity);
    }

    public async Task<Result<long>> Remove(Expression<Func<TEntity, bool>> predicate)
    {
      return await _repository.Remove(predicate);
    }

    public async Task<Result<TEntity>> Update(TEntity entity)
    {
      return await _repository.Update(entity);
    }
  }
}