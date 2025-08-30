using Microsoft.Data.Sqlite;

namespace PersonasCRUD.Infrastructure.Persistence;

public class SqliteConnectionFactory
{
    private readonly string _connectionString;

    public SqliteConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Devuelve la implementación concreta de Sqlite
    public SqliteConnection CreateConnection()
    {
        return new SqliteConnection(_connectionString);
    }
}

