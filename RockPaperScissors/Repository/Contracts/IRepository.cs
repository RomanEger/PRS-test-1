using System.Linq.Expressions;

namespace Repository.Contracts;

public interface IRepository<T> where T: class
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T> GetAsync(Expression<Func<T, bool>> condition);

    Task<int> AddAsync(T newEntity);

    Task<int> UpdateAsync(T entity);

    Task<int> DeleteAsync(T entity);
}