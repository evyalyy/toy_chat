using Microsoft.Data.Sqlite;
using Server.Utils;

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

        var query = "INSERT INTO Messages (Content,UserId, SentTs)" +
                    "VALUES (@content, @userId, @ts)" +
                    "RETURNING Id";
        using var command = new SqliteCommand(query, conn);
        command.Parameters.Add(new SqliteParameter("userId", senderId.ToString()));
        command.Parameters.Add(new SqliteParameter("content", content));
        command.Parameters.Add(new SqliteParameter("ts", timestamp.ToUnixTime()));
        var output = command.ExecuteScalar();
        if (output is not null)
        {
            return (int)(long)output;
        }

        throw new Exception("Cannot insert message");
    }

    public List<Message> GetMessages(int fromId)
    {
        var messages = new List<Message>();
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        const string query = "SELECT Id, Content, UserId, SentTs FROM Messages WHERE Id >= @id";
        using var command = new SqliteCommand(query, conn);
        command.Parameters.Add(new SqliteParameter("id", fromId));
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            messages.Add(new Message
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Content = (string)reader["Content"],
                UserId = new UserUuid((string)reader["UserId"]),
                SentTs = reader.GetInt64(reader.GetOrdinal("SentTs")).FromUnixTime()
            });
        }

        return messages;
    }
}