using System.Linq.Expressions;
using CurrencyTracker.Domain.Interfaces;
using CurrencyTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CurrencyTracker.Infrastructure.Implementations;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet;
    private readonly AppDbContext _context;

    public GenericRepository(AppDbContext context) {
        
        _context = context;
        _dbSet=_context.Set<T>();

    }
    public async Task<T> AddAsync(T entity)
    {
         await _dbSet.AddAsync(entity);
         return entity;
    }

    public IQueryable<T> GetAll()
    {
        return _dbSet.AsNoTracking();
    }

    public async Task<T?> GetByIdAsync(Guid Id)
    {
        return await _dbSet.FindAsync(Id);

    }
    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public Task SaveAsync()
    {
       return _context.SaveChangesAsync();
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate).AsNoTracking();
    }
}
