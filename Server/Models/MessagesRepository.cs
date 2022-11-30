using Microsoft.Data.Sqlite;

namespace Server.Models;

public class MessagesRepository: IMessagesRepository
{
    private readonly string _connectionString;

    public MessagesRepository(IConfiguration configuration)
    {
        _connectionString = configuration["DbConnectionString"];
    }
    public int AddMessage(UserUuid senderId, string content, DateTime timestamp)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        var query = "INSERT INTO Messages (Content,UserId)" +
                    "VALUES (@content, @userId)" +
                    "RETURNING Id";
        using var command = new SqliteCommand(query, conn);
        command.Parameters.Add(new SqliteParameter("userId", senderId.ToString()));
        command.Parameters.Add(new SqliteParameter("content", content));
        var output = command.ExecuteScalar();
        if (output is not null)
        {
            return (int)output;
        }

        throw new Exception("Cannot insert message");
    }

    public List<Message> GetMessages(int fromId)
    {
        var messages = new List<Message>();
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        const string query = "SELECT Id, Content, UserId FROM Messages WHERE Id >= @id";
        using var command = new SqliteCommand(query, conn);
        command.Parameters.Add(new SqliteParameter("id", fromId));
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            messages.Add(new Message
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Content = (string)reader["Content"],
                UserId = new UserUuid((string)reader["UserId"])
            });
        }

        return messages;
    }
}