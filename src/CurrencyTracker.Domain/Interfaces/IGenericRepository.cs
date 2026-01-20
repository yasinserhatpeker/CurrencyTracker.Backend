using System;
using System.Linq.Expressions;
namespace CurrencyTracker.Domain.Interfaces;

public interface IGenericRepository <T> where T : class
{
    Task<T?> GetByIdAsync(Guid Id);
    IQueryable<T> GetAll();
    IQueryable<T> Where(Expression<Func<T,bool>> predicate);
    Task<T> AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);

}
