using Microsoft.Data.Sqlite;
using PersonasCRUD.Domain.Entities;
using PersonasCRUD.Domain.Interfaces;
using System.Data;
using PersonasCRUD.Domain.Enums;

namespace PersonasCRUD.Infrastructure.Persistence;

public class PersonaRepository : IPersonaRepository
{
    private readonly SqliteConnectionFactory _factory;

    /**
     * INYECCION DE DEPENDENCIAS EN EL CONSTRUCTOR
     */
    public PersonaRepository(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    /**
     * METODO QUE AGREGA UNA PERSONA A LA BASE DE DATOS
     */
    public async Task<Persona> AddAsync(Persona persona)
    {
        using var conn = _factory.CreateConnection();
        await conn.OpenAsync();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        INSERT INTO Personas (Nombre, Apellido, TipoPersona, Telefono, Email, FechaNacimiento)
        VALUES (@nombre, @apellido, @tipoPersona, @telefono, @email, @fecha);
    ";

        cmd.Parameters.AddWithValue("@nombre", persona.Nombre);
        cmd.Parameters.AddWithValue("@apellido", persona.Apellido);
        cmd.Parameters.AddWithValue("@tipoPersona", (int)persona.TipoPersona);
        cmd.Parameters.AddWithValue("@telefono", persona.Telefono);
        cmd.Parameters.AddWithValue("@email", persona.Email);
        cmd.Parameters.AddWithValue("@fecha", persona.FechaNacimiento.ToString("yyyy-MM-dd"));

        await cmd.ExecuteNonQueryAsync();

        // Recuperar el último Id generado
        cmd.CommandText = "SELECT last_insert_rowid()";
        persona.Id = Convert.ToInt32(await cmd.ExecuteScalarAsync());

        return persona;
    }


    
    /**
     * METODO QUE ELIMINA UNA PERSONA DE LA BASE DE DATOS
     */
    public async Task<bool> DeleteAsync(int id)
    {
        using var conn = _factory.CreateConnection();
        await conn.OpenAsync();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Personas WHERE Id = @id";
        cmd.Parameters.AddWithValue("@id", id);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    
    /**
     * METODO QUE LISTA TODAS LAS PERSONAS DE LA BASE DE DATOS
     */
    public async Task<IEnumerable<Persona>> GetAllAsync()
    {
        var personas = new List<Persona>();

        using var conn = _factory.CreateConnection();
        await conn.OpenAsync();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Nombre, Apellido, TipoPersona, Telefono, Email,  FechaNacimiento FROM Personas";

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            personas.Add(new Persona
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                Apellido = reader.GetString(2),
                TipoPersona = (TipoPersona)reader.GetInt32(3),
                Telefono = reader.GetString(4),
                Email = reader.GetString(5),
                FechaNacimiento = DateTime.Parse(reader.GetString(6))
            });
        }

        return personas;
    }

    
    /**
     * METODO QUE BUSCA UNA PERSONA POR ID
     */
    public async Task<Persona?> GetByIdAsync(int id)
    {
        using var conn = _factory.CreateConnection();
        await conn.OpenAsync();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Nombre, Apellido, TipoPersona, Telefono, Email,  FechaNacimiento FROM Personas WHERE Id = @id";
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Persona
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                Apellido = reader.GetString(2),
                TipoPersona = (TipoPersona)reader.GetInt32(3),
                Telefono = reader.GetString(4),
                Email = reader.GetString(5),
                FechaNacimiento = DateTime.Parse(reader.GetString(6))
            };
        }

        return null;
    }

    
    /**
     * METODO QUE ACTUALIZA UNA PERSONA EN LA BASE DE DATOS
     */   
    public async Task<bool> UpdateAsync(Persona persona)
    {
        using var conn = _factory.CreateConnection();
        await conn.OpenAsync();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
        UPDATE Personas SET 
            Nombre = @nombre, 
            Apellido = @apellido, 
            TipoPersona = @tipo, 
            Telefono = @telefono, 
            Email = @email, 
            FechaNacimiento = @fecha 
        WHERE Id = @id;
    ";

        cmd.Parameters.AddWithValue("@nombre", persona.Nombre);
        cmd.Parameters.AddWithValue("@apellido", persona.Apellido);
        cmd.Parameters.AddWithValue("@tipo", (int)persona.TipoPersona);
        cmd.Parameters.AddWithValue("@telefono", persona.Telefono);
        cmd.Parameters.AddWithValue("@email", persona.Email);
        cmd.Parameters.AddWithValue("@fecha", persona.FechaNacimiento.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("@id", persona.Id);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }


    
    // Métodos específicos de IPersonaRepository
    
    /**
     * METODO QUE BUSCA UNA PERSONA POR RANGO DE EDAD
     */   
    public async Task<IEnumerable<Persona>> GetByAgeRangeAsync(int minAge, int maxAge)
    {
        var personas = await GetAllAsync();
        return personas.Where(p => p.Edad >= minAge && p.Edad <= maxAge);
    }
    
    
    /**
     * METODO QUE BUSCA UNA PERSONA POR NOMBRE (PARCIAL)
     */  
    public async Task<IEnumerable<Persona>> SearchByNameAsync(string name)
    {
        var personas = await GetAllAsync();
        return personas.Where(p => p.Nombre.Contains(name, StringComparison.OrdinalIgnoreCase));
    }

    
    /**
     * METODO QUE BUSCA UNA PERSONA POR NOMBRE (EXACTO)
     */ 
    public async Task<Persona?> GetByNombreAsync(string nombre)
    {
        var personas = await GetAllAsync();
        return personas.FirstOrDefault(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
    }
}
