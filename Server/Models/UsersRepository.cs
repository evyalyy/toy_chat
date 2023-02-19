using Microsoft.Data.Sqlite;

namespace Server.Models;

public class UsersRepository : IUsersRepository
{
    private readonly string _connectionString;

    public UsersRepository(IConfiguration configuration)
    {
        _connectionString = configuration["DbConnectionString"];
    }

    public long AddUser(string phoneNumber, string password, string name)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        const string query = @"
            INSERT INTO Users (PhoneNumber,Name,Password)
            VALUES (@phone, @name, @password)
            RETURNING Id";
        using var command = new SqliteCommand(query, conn);
        command.Parameters.Add(new SqliteParameter("phone", phoneNumber));
        command.Parameters.Add(new SqliteParameter("name", name));
        command.Parameters.Add(new SqliteParameter("password", password));
        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            throw new Exception("No user id returned");
        }

        return (long)reader["Id"];
    }

    public User? GetUser(long id)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        const string query = "SELECT Id, PhoneNumber, Name, Password FROM Users WHERE Id = @id LIMIT 1";
        using var command = new SqliteCommand(query, conn);
        command.Parameters.Add(new SqliteParameter("id", id));
        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        var user = new User
            ((string)reader["Name"], (string)reader["Password"], (string)reader["PhoneNumber"]);

        return user;
    }
}