using System;
using System.Linq.Expressions;
namespace CurrencyTracker.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid Id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(Guid Id);
    Task SaveAsync();

}
