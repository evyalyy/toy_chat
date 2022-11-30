using Microsoft.Data.Sqlite;

namespace Server.Models;

public class UsersRepository : IUsersRepository
{
    private string ConnectionString;

    public UsersRepository(IConfiguration configuration)
    {
        ConnectionString = configuration["DbConnectionString"];
    }

    public UserUuid AddUser(UserUuid id, string phoneNumber, string password, string name)
    {
        using (var conn = new SqliteConnection(ConnectionString))
        {
            conn.Open();

            var query = "INSERT INTO Users (Id,PhoneNumber,Name,Password)" +
                        "VALUES (@id, @phone, @name, @password)" +
                        "RETURNING Id";
            using (var command = new SqliteCommand(query, conn))
            {
                command.Parameters.Add(new SqliteParameter("id", id.ToString()));
                command.Parameters.Add(new SqliteParameter("phone", phoneNumber));
                command.Parameters.Add(new SqliteParameter("name", name));
                command.Parameters.Add(new SqliteParameter("password", password));
                var output = command.ExecuteScalar();
                Console.WriteLine($"Output: {output}");
                if (output is not null)
                {
                    return new UserUuid((string)output);
                }

                return UserUuid.Empty();
            }
        }
    }

    public User? GetUser(UserUuid id)
    {
        using (var conn = new SqliteConnection(ConnectionString))
        {
            conn.Open();

            var query = "SELECT Id, PhoneNumber, Name, Password FROM Users WHERE Id = @id LIMIT 1";
            using (var command = new SqliteCommand(query, conn))
            {
                command.Parameters.Add(new SqliteParameter("id", id.ToString()));
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var user = new User();
                        user.Id = new UserUuid((string)reader["Id"]);
                        user.PhoneNumber = (string)reader["PhoneNumber"];
                        user.Name = (string)reader["Name"];
                        user.Password = (string)reader["Password"];
                        return user;
                    }

                    return null;
                }
            }
        }
    }
}