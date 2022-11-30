using Microsoft.Data.Sqlite;

namespace Server.Utils;

public class MigrationsRunner
{
    private readonly string _connectionString;
    private readonly List<IMigration> _migrations;

    public MigrationsRunner(string connectionString)
    {
        _connectionString = connectionString;
        _migrations = new List<IMigration>();
    }

    public void Add(IMigration migration)
    {
        _migrations.Add(migration);
    }

    private void CreateTable(SqliteConnection conn)
    {
        const string query = @"
            CREATE TABLE IF NOT EXISTS _migrations (
                Id TEXT PRIMARY KEY,
                AppliedAt INTEGER NOT NULL
            )";
        using var command = new SqliteCommand(query, conn);
        command.ExecuteNonQuery();
    }

    List<string> GetAppliedMigrations(SqliteConnection conn)
    {
        const string query = @"
            SELECT Id FROM _migrations;
";
        using var command = new SqliteCommand(query, conn);
        using var reader = command.ExecuteReader();
        var outMigrations = new List<string>();
        while (reader.Read())
        {
            outMigrations.Add(reader.GetString(0));
        }

        return outMigrations;
    }

    private static void ApplyMigration(
        SqliteConnection conn,
        IMigration migration,
        List<string> appliedMigrations)
    {
        var name = migration.GetType().ToString();
        if (appliedMigrations.Contains(name))
        {
            Console.WriteLine($"Skip applied migration {name}");
            return;
        }
        Console.WriteLine($"Applying migration {name}");
        new SqliteCommand(migration.UpCommand(), conn).ExecuteNonQuery();
        const string recordMigration = @"
            INSERT INTO _migrations (Id, AppliedAt) VALUES ($id, $ts);
        ";
        using var command = new SqliteCommand(recordMigration, conn);
        command.Parameters.Add(new SqliteParameter("id", name));
        command.Parameters.Add(new SqliteParameter("ts", DateTime.Now.ToUnixTime()));
        command.ExecuteNonQuery();
    }

    public void Up()
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        CreateTable(conn);

        var appliedMigrations = GetAppliedMigrations(conn);

        foreach (var migration in _migrations)
        {
            ApplyMigration(conn, migration, appliedMigrations);
        }
    }
}