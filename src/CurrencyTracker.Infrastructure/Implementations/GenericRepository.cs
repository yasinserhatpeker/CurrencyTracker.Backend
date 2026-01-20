using System;
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
    public Task<T> AddAsync(T entity)
    {
        
    }

    public IQueryable<T> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<T> GetByIdAsync(int Id)
    {
        throw new NotImplementedException();
    }

    public void Remove(T entity)
    {
        throw new NotImplementedException();
    }

    public void Update(T entity)
    {
        throw new NotImplementedException();
    }

    public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}
