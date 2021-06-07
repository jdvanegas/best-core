using Domain.Common;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess
{
  public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
  {
    protected readonly DbContext _context;
    protected readonly DbSet<TEntity> _dbSet;
    protected readonly ILogger _logger;

    public Repository(DbContext dbContext, ILogger<Repository<TEntity>> logger)
    {
      _context = dbContext;
      _logger = logger;
      _dbSet = _context.Set<TEntity>();
    }

    public void Dispose()
    {
      _context.Dispose();
      GC.SuppressFinalize(this);
    }

    public async Task<Result<TEntity>> Create(TEntity entity)
    {
      _dbSet.Add(entity);
      var result = new Result<TEntity>();
      try
      {
        await _context.SaveChangesAsync();
        result.Data = entity;
        return result;
      }
      catch (Exception e)
      {
        result.Errors.Add(e.Message);
        _logger.LogError(e.Message);
        if (e.InnerException != null)
        {
          result.Errors.Add(e.InnerException.Message);
          _logger.LogError(e.InnerException.Message);
        }
        return result;
      }
    }

    public async Task<Result<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
    {
      var result = new Result<TEntity>();
      result.Data = await _dbSet.FirstOrDefaultAsync(predicate);
      if (result.Data == null) result.Errors.Add("Not found object");
      return result;
    }

    public IQueryable<TEntity> Queryable(bool @readonly = true)
    {
      return @readonly ? _dbSet.AsNoTracking().AsQueryable() : _dbSet.AsQueryable();
    }

    public IQueryable<TEntity> Queryable(Expression<Func<TEntity, bool>> predicate, bool @readonly = true)
    {
      return @readonly ? _dbSet.Where(predicate).AsNoTracking().AsQueryable() : _dbSet.Where(predicate).AsQueryable();
    }

    public async Task<Result<bool>> Remove(TEntity entity)
    {
      if (_context.Entry(entity).State == EntityState.Detached)
      {
        _dbSet.Attach(entity);
      }
      _dbSet.Remove(entity);
      var result = new Result<bool>();
      try
      {
        await _context.SaveChangesAsync();
        result.Data = true;
      }
      catch (Exception e)
      {
        result.Errors.Add(e.Message);
        _logger.LogError(e.Message);
        if (e.InnerException != null)
        {
          result.Errors.Add(e.InnerException.Message);
          _logger.LogError(e.InnerException.Message);
        }
      }
      return result;
    }

    public async Task<Result<long>> Remove(Expression<Func<TEntity, bool>> predicate)
    {
      var entities = _dbSet.Where(predicate);
      var result = new Result<long>();
      if (!entities.Any())
      {
        result.Errors.Add("Not found objects");
        return result;
      }
      foreach (var entity in entities)
      {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
          _dbSet.Attach(entity);
        }
        _dbSet.Remove(entity);
      }
      try
      {
        await _context.SaveChangesAsync();
      }
      catch (Exception e)
      {
        result.Errors.Add(e.Message);
        _logger.LogError(e.Message);
        if (e.InnerException != null)
        {
          result.Errors.Add(e.InnerException.Message);
          _logger.LogError(e.InnerException.Message);
        }
      }

      return result;
    }

    public async Task<Result<TEntity>> Update(TEntity entity)
    {
      _dbSet.Attach(entity);
      _context.Entry(entity).State = EntityState.Modified;
      var result = new Result<TEntity>();
      try
      {
        await _context.SaveChangesAsync();
        result.Data = entity;
      }
      catch (Exception e)
      {
        result.Errors.Add(e.Message);
        _logger.LogError(e.Message);
        if (e.InnerException != null)
        {
          result.Errors.Add(e.InnerException.Message);
          _logger.LogError(e.InnerException.Message);
        }
      }
      return result;
    }
  }
}