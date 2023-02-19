using Microsoft.Data.Sqlite;
using Server.Utils;

namespace Server.Models;

public class ChannelsRepository : IChannelsRepository
{
    private readonly string _connectionString;
    private readonly IMessagesRepository _messages;

    public ChannelsRepository(IConfiguration configuration, IMessagesRepository messages)
    {
        _connectionString = configuration["DbConnectionString"];
        _messages = messages;
    }

    public Channel AddChannel()
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        long channelId = 0;
        {
            var query = "INSERT INTO Channels DEFAULT VALUES RETURNING Id";
            using var command = new SqliteCommand(query, conn);
            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                throw new Exception("Cannot create new channel");
            }

            channelId = (long)reader["Id"];
        }

        var channel = GetChannel(channelId);

        if (channel is null)
        {
            throw new Exception($"WTF? Cannot select created channel {channelId}");
        }

        return channel;
    }

    public void UpdateChannel(long channelId, int lastMessageId, DateTime lastMessageTs)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        var query = @"
            UPDATE Channels
            SET LastMessageId = @lastMessageId, LastMessageTs = @lastMessageTs
            WHERE Id = @id";
        using var command = new SqliteCommand(query, conn);
        command.Parameters.Add(new SqliteParameter("id", channelId));
        command.Parameters.Add(new SqliteParameter("lastMessageId", lastMessageId));
        command.Parameters.Add(new SqliteParameter("lastMessageTs", lastMessageTs.ToUnixTime()));
        var numRowsAffected = command.ExecuteNonQuery();
        if (numRowsAffected != 1)
        {
            throw new Exception("Cannot update channel");
        }
    }

    public Channel? GetChannel(long channelId)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        const string query = "SELECT Id, LastMessageId, LastMessageTs FROM Channels WHERE Id = @id LIMIT 1";
        using var command = new SqliteCommand(query, conn);
        command.Parameters.Add(new SqliteParameter("id", channelId));
        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        return null;
    }
}