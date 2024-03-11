using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository;

public class GenericRepository<T>(RpstestContext dbContext) : IRepository<T> where T: class
{
    
    public async Task<IEnumerable<T>> GetAllAsync() => await dbContext.Set<T>().ToListAsync();

    public async Task<T> GetAsync(Expression<Func<T, bool>> condition) => await dbContext.Set<T>().FirstOrDefaultAsync(condition);

    public async Task<int> AddAsync(T newEntity)
    {
        
        await dbContext.Set<T>().AddAsync(newEntity);
        return await dbContext.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(T entity)
    {
        dbContext.Set<T>().Update(entity);
        return await dbContext.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(T entity)
    {
        dbContext.Set<T>().Remove(entity);
        return await dbContext.SaveChangesAsync();
    }
}