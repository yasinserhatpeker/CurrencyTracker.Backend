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
         await _context.SaveChangesAsync();
         return entity;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid Id)
    {
        return await _dbSet.FindAsync(Id);

    }
    public async Task<T> DeleteAsync(Guid Id)
    {
        var entity = await _dbSet.FindAsync(Id);
        if(entity is null)
        {
            return null!;
        }
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;

    }

    public Task SaveAsync()
    {
       return _context.SaveChangesAsync();
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).AsNoTracking().ToListAsync();
    }
}
