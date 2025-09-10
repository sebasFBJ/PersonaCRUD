using PersonasCRUD.Application.DTOs;
using PersonasCRUD.Domain.Entities;

namespace PersonasCRUD.Application.Interfaces
{
    public interface IPersonaService
    {
        // Crear una nueva persona
        Task<PersonaDto> CrearPersonaAsync(PersonaDto dto);

        // Obtener todas las personas
        Task<IEnumerable<PersonaDto>> ObtenerTodasAsync();

        // Buscar persona por ID
        Task<PersonaDto?> BuscarPorIdAsync(int id);

        // Actualizar persona
        Task<bool> ActualizarPersonaAsync(int id, PersonaDto dto);

        // Eliminar persona
        Task<bool> EliminarPersonaAsync(int id);

        // Buscar personas por nombre (parcial)
        Task<IEnumerable<PersonaDto>> BuscarPorNombreAsync(string nombre);

        // Buscar personas por rango de edad
        Task<IEnumerable<PersonaDto>> BuscarPorRangoEdadAsync(int edadMin, int edadMax);
    }
}