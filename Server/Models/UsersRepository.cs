using Microsoft.Data.Sqlite;

namespace Server.Models;

public class UsersRepository : IUsersRepository
{
    private readonly string _connectionString;

    public UsersRepository(IConfiguration configuration)
    {
        _connectionString = configuration["DbConnectionString"];
    }

    public UserUuid AddUser(string phoneNumber, string password, string name)
    {
        var userId = UserUuid.New();
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        const string query = @"
            INSERT INTO Users (Id,PhoneNumber,Name,Password)
            VALUES (@id, @phone, @name, @password)";
        using var command = new SqliteCommand(query, conn);
        command.Parameters.Add(new SqliteParameter("id", userId.ToString()));
        command.Parameters.Add(new SqliteParameter("phone", phoneNumber));
        command.Parameters.Add(new SqliteParameter("name", name));
        command.Parameters.Add(new SqliteParameter("password", password));
        var numRowsAffected = command.ExecuteNonQuery();
        if (numRowsAffected != 1)
        {
            throw new Exception($"Cannot add new user with id {userId}");
        }

        return userId;
    }

    public User? GetUser(UserUuid id)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        const string query = "SELECT Id, PhoneNumber, Name, Password FROM Users WHERE Id = @id LIMIT 1";
        using var command = new SqliteCommand(query, conn);
        command.Parameters.Add(new SqliteParameter("id", id.ToString()));
        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }
        var user = new User
        {
            Id = new UserUuid((string)reader["Id"]),
            PhoneNumber = (string)reader["PhoneNumber"],
            Name = (string)reader["Name"],
            Password = (string)reader["Password"]
        };
        return user;

    }
}