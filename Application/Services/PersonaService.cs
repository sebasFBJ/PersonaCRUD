using PersonasCRUD.Domain.Entities;
using PersonasCRUD.Domain.Interfaces;

namespace PersonasCRUD.Application.Services;

public class PersonaService
{
    private readonly IPersonaRepository _personaRepository;

    // Inyectamos la dependencia del repositorio (no creamos el repo aquí).
    public PersonaService(IPersonaRepository personaRepository)
    {
        _personaRepository = personaRepository;
    }

    // Agregar persona con validaciones
    public async Task<Persona> CrearPersonaAsync(string nombre, DateTime fechaNacimiento)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre no puede estar vacío.");

        if (fechaNacimiento > DateTime.Today)
            throw new ArgumentException("La fecha de nacimiento no puede ser futura.");

        var persona = new Persona
        {
            Nombre = nombre,
            FechaNacimiento = fechaNacimiento
        };

        return await _personaRepository.AddAsync(persona);
    }

    // Listar todas las personas
    public async Task<IEnumerable<Persona>> ObtenerTodasAsync()
    {
        return await _personaRepository.GetAllAsync();
    }

    // Buscar por ID
    public async Task<Persona?> BuscarPorIdAsync(int id)
    {
        return await _personaRepository.GetByIdAsync(id);
    }

    // Actualizar persona
    public async Task<bool> ActualizarPersonaAsync(Persona persona)
    {
        return await _personaRepository.UpdateAsync(persona);
    }

    // Eliminar persona
    public async Task<bool> EliminarPersonaAsync(int id)
    {
        return await _personaRepository.DeleteAsync(id);
    }

    // Buscar por nombre
    public async Task<IEnumerable<Persona>> BuscarPorNombreAsync(string nombre)
    {
        return await _personaRepository.SearchByNameAsync(nombre);
    }

    // Buscar por rango de edad
    public async Task<IEnumerable<Persona>> BuscarPorRangoEdadAsync(int min, int max)
    {
        return await _personaRepository.GetByAgeRangeAsync(min, max);
    }
}
