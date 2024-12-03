using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

using TestCadastroVendas.Domain.Repositories.Sql;
using TestCadastroVendas.Infra.Persistence.Sql.Contexts;

namespace TestCadastroVendas.Infra.Persistence.Sql.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DataContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(DataContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();  // Recupera o DbSet da entidade genérica
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null)
    {
        IQueryable<T> query = _dbSet;

        if (predicate != null)
        {
            query = query.Where(predicate); 
        }

        return await query.ToListAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
    {
        // Aplica o filtro
        var entity = await _dbSet.FirstOrDefaultAsync(predicate);

        // Verifica se a entidade foi encontrada
        if (entity != null)
        {
            _dbSet.Remove(entity);  // Remove a entidade
            await _context.SaveChangesAsync();  // Salva as alterações no contexto
        }
        else
        {
            throw new KeyNotFoundException("A entidade não foi encontrada com o filtro especificado.");
        }
    }
}

