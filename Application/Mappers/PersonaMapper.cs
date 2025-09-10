using PersonasCRUD.Application.DTOs;
using PersonasCRUD.Domain.Entities;
using PersonasCRUD.Domain.Enums;

public static class PersonaMapper
{
    public static Persona ToEntity(PersonaDto dto)
    {
        return new Persona
        {
            Id = dto.Id,
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            TipoPersona = (TipoPersona)dto.TipoPersona,
            Telefono = dto.Telefono,
            Email = dto.Email,
            FechaNacimiento = dto.FechaNacimiento
        };
    }

    public static PersonaDto ToDto(Persona persona)
    {
        return new PersonaDto
        {
            Id = persona.Id,
            Nombre = persona.Nombre,
            Apellido = persona.Apellido,
            TipoPersona = (int)persona.TipoPersona,
            Telefono = persona.Telefono,
            Email = persona.Email,
            FechaNacimiento = persona.FechaNacimiento
        };
    }
    
    public static IEnumerable<PersonaDto> ToDtoList(IEnumerable<Persona> personas)
    {
        return personas.Select(p => ToDto(p));
    }

    public static void MapToExistingEntity(PersonaDto dto, Persona persona)
    {
        persona.Nombre = dto.Nombre;
        persona.Apellido = dto.Apellido;
        persona.TipoPersona = (TipoPersona)dto.TipoPersona;
        persona.Telefono = dto.Telefono;
        persona.Email = dto.Email;
        persona.FechaNacimiento = dto.FechaNacimiento;
    }

}