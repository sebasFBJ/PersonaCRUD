using PersonasCRUD.Domain.Entities;

namespace PersonasCRUD.Domain.Interfaces;

public interface IPersonaRepository : IGenericRepository<Persona>
{
    // Este metodo busca personas por un rango de edad
    Task<IEnumerable<Persona>> GetByAgeRangeAsync(int minAge, int maxAge);
    // Este metodo busca personas por nombre (busqueda parcial)
    Task<IEnumerable<Persona>> SearchByNameAsync(string name);
    // Este metodo busca personas por nombre exacto
    Task<Persona?> GetByNombreAsync(string nombre);
}