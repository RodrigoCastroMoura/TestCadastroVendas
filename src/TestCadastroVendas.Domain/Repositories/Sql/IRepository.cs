using System.Linq.Expressions;

namespace TestCadastroVendas.Domain.Repositories.Sql;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task DeleteAsync(Expression<Func<T, bool>> predicate);
}
