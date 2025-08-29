namespace PersonasCRUD.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    
    Task<IEnumerable<T>> GetAllAsync();

    Task AddAsync(T entity);

    Task<bool> UpdateAsync(T entity);

    Task<bool> DeleteAsync(int id);
}