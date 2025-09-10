using System.Data;

namespace PersonasCRUD.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static void Initialize(SqliteConnectionFactory factory)
    {
        using var conn = factory.CreateConnection();
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Personas (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre VARCHAR NOT NULL,
                Apellido VARCHAR NOT NULL,
                TipoPersona INTEGER NOT NULL,
                Telefono VARCHAR NOT NULL,
                Email VARCHAR NOT NULL,
                FechaNacimiento DATE NOT NULL
            );
        ";
        cmd.ExecuteNonQuery();
    }
}
