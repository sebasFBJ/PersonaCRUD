using System.ComponentModel.DataAnnotations;
using PersonasCRUD.Application.DTOs;
using PersonasCRUD.Application.Interfaces;
using PersonasCRUD.Domain.Entities;
using PersonasCRUD.Domain.Enums;
using PersonasCRUD.Domain.Interfaces;


namespace PersonasCRUD.Application.Services;

public class PersonaService : IPersonaService
{
    private readonly IPersonaRepository _personaRepository;

    /**
     * Inyecion de dependencias
     */
    public PersonaService(IPersonaRepository personaRepository)
    {
        _personaRepository = personaRepository;
    }

    /**
     * Metodo que crea una nueva persona en la base de datos
     */
    public async Task<PersonaDto> CrearPersonaAsync(PersonaDto dto)
    {
        ValidarDto(dto);
        
        var persona = PersonaMapper.ToEntity(dto);

        return PersonaMapper.ToDto(await _personaRepository.AddAsync(persona));
    }

    /**
     * Obtener todas las personas de la base de datos
     */
    public async Task<IEnumerable<PersonaDto>> ObtenerTodasAsync()
    {
        return PersonaMapper.ToDtoList(await _personaRepository.GetAllAsync());
    }

    /**
     * Buscar persona por ID
     */   
    public async Task<PersonaDto?> BuscarPorIdAsync(int id)
    {
        return PersonaMapper.ToDto(await _personaRepository.GetByIdAsync(id));
    }

    /**
     * Actualizar persona
     */  
    public async Task<bool> ActualizarPersonaAsync(int id, PersonaDto dto)
    {
        ValidarDto(dto);

        var perona = PersonaMapper.ToEntity(dto); 

        return await _personaRepository.UpdateAsync(perona);
    }

    /**
     * Eliminar persona
     */ 
    public async Task<bool> EliminarPersonaAsync(int id)
    {
        return await _personaRepository.DeleteAsync(id);
    }

    /**
     * Buscar persona por nombre
     */
    public async Task<IEnumerable<PersonaDto>> BuscarPorNombreAsync(string nombre)
    {
        return PersonaMapper.ToDtoList(await _personaRepository.SearchByNameAsync(nombre));
    }

    /**
     * Buscar persona por rango de edad
     */
    public async Task<IEnumerable<PersonaDto>> BuscarPorRangoEdadAsync(int edadMin, int edadMax)
    {
        return PersonaMapper.ToDtoList(await _personaRepository.GetByAgeRangeAsync(edadMin, edadMax));;
    }

    // 🔍 Validación manual del DTO
    private void ValidarDto(PersonaDto dto)
    {
        var context = new ValidationContext(dto);
        var results = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(dto, context, results, true);

        if (!isValid)
        {
            var errores = string.Join("; ", results.Select(r => r.ErrorMessage));
            throw new ValidationException($"Errores de validación: {errores}");
        }

        if (dto.FechaNacimiento > DateTime.Today)
            throw new ArgumentException("La fecha de nacimiento no puede ser futura.");
    }
}
