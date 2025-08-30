using Microsoft.Data.Sqlite;
using PersonasCRUD.Domain.Entities;
using PersonasCRUD.Domain.Interfaces;
using System.Data;

namespace PersonasCRUD.Infrastructure.Persistence;

public class PersonaRepository : IPersonaRepository
{
    private readonly SqliteConnectionFactory _factory;

    public PersonaRepository(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<Persona> AddAsync(Persona persona)
    {
        using var conn = _factory.CreateConnection();
        await conn.OpenAsync();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT INTO Personas (Nombre, FechaNacimiento) VALUES (@nombre, @fecha)";
        cmd.Parameters.AddWithValue("@nombre", persona.Nombre);
        cmd.Parameters.AddWithValue("@fecha", persona.FechaNacimiento.ToString("yyyy-MM-dd"));

        await cmd.ExecuteNonQueryAsync();

        // Recuperar el último Id generado
        cmd.CommandText = "SELECT last_insert_rowid()";
        persona.Id = Convert.ToInt32(await cmd.ExecuteScalarAsync());

        return persona;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var conn = _factory.CreateConnection();
        await conn.OpenAsync();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Personas WHERE Id = @id";
        cmd.Parameters.AddWithValue("@id", id);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<IEnumerable<Persona>> GetAllAsync()
    {
        var personas = new List<Persona>();

        using var conn = _factory.CreateConnection();
        await conn.OpenAsync();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Nombre, FechaNacimiento FROM Personas";

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            personas.Add(new Persona
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                FechaNacimiento = DateTime.Parse(reader.GetString(2))
            });
        }

        return personas;
    }

    public async Task<Persona?> GetByIdAsync(int id)
    {
        using var conn = _factory.CreateConnection();
        await conn.OpenAsync();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Nombre, FechaNacimiento FROM Personas WHERE Id = @id";
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Persona
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                FechaNacimiento = DateTime.Parse(reader.GetString(2))
            };
        }

        return null;
    }

    public async Task<bool> UpdateAsync(Persona persona)
    {
        using var conn = _factory.CreateConnection();
        await conn.OpenAsync();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Personas SET Nombre = @nombre, FechaNacimiento = @fecha WHERE Id = @id";
        cmd.Parameters.AddWithValue("@nombre", persona.Nombre);
        cmd.Parameters.AddWithValue("@fecha", persona.FechaNacimiento.ToString("yyyy-MM-dd"));
        cmd.Parameters.AddWithValue("@id", persona.Id);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    // Métodos específicos de IPersonaRepository
    public async Task<IEnumerable<Persona>> GetByAgeRangeAsync(int minAge, int maxAge)
    {
        var personas = await GetAllAsync();
        return personas.Where(p => p.Edad >= minAge && p.Edad <= maxAge);
    }

    public async Task<IEnumerable<Persona>> SearchByNameAsync(string name)
    {
        var personas = await GetAllAsync();
        return personas.Where(p => p.Nombre.Contains(name, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<Persona?> GetByNombreAsync(string nombre)
    {
        var personas = await GetAllAsync();
        return personas.FirstOrDefault(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
    }
}
