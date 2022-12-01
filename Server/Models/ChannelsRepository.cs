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

        var channelId = ChannelId.New();
        
        var query = "INSERT INTO Channels (Id) VALUES (@id)";
        using var command = new SqliteCommand(query, conn);
        command.Parameters.Add(new SqliteParameter("id", channelId.ToString()));
        var numRowsAffected = command.ExecuteNonQuery();
        if (numRowsAffected != 1)
        {
            throw new Exception($"Cannot create channel {channelId}");
        }

        var channel = GetChannel(channelId);

        if (channel is null)
        {
            throw new Exception($"WTF? Cannot select created channel {channelId}");
        }
        return channel;
    }

    public Channel AddPrivateChannel(UserUuid userId1, UserUuid userId2)
    {
        var newChannel = AddChannel();
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        var query = @"
            INSERT INTO PrivateChannels (UserId1, UserId2, ChannelId)
            VALUES (@userId1, @userId2, @channelId)";
        using var command = new SqliteCommand(query, conn);
        command.Parameters.Add(new SqliteParameter("userId1", userId1.ToString()));
        command.Parameters.Add(new SqliteParameter("userId2", userId2.ToString()));
        command.Parameters.Add(new SqliteParameter("channelId", newChannel.Id.ToString()));
        var numRowsAffected = command.ExecuteNonQuery();
        if (numRowsAffected != 1)
        {
            throw new Exception("Cannot insert private channel");
        }

        return newChannel;
    }

    public void UpdateChannel(ChannelId channelId, int lastMessageId, DateTime lastMessageTs)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        var query = @"
            UPDATE Channels
            SET LastMessageId = @lastMessageId, LastMessageTs = @lastMessageTs
            WHERE Id = @id";
        using var command = new SqliteCommand(query, conn);
        command.Parameters.Add(new SqliteParameter("id", channelId.ToString()));
        command.Parameters.Add(new SqliteParameter("lastMessageId", lastMessageId));
        command.Parameters.Add(new SqliteParameter("lastMessageTs", lastMessageTs.ToUnixTime()));
        var numRowsAffected = command.ExecuteNonQuery();
        if (numRowsAffected != 1)
        {
            throw new Exception("Cannot update channel");
        }
    }

    public Channel? GetChannel(ChannelId channelId)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        const string query = "SELECT Id, LastMessageId, LastMessageTs FROM Channels WHERE Id = @id LIMIT 1";
        using var command = new SqliteCommand(query, conn);
        var chStr = channelId.ToString();
        command.Parameters.Add(new SqliteParameter("id", channelId.ToString()));
        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        return new Channel
        (
            _messages,
            this,
            new ChannelId((string)reader["Id"]),
            reader.GetInt32(reader.GetOrdinal("LastMessageId")),
            reader.GetInt64(reader.GetOrdinal("LastMessageTs")).FromUnixTime()
        );

    }

    public Channel? GetPrivateChannel(UserUuid userId1, UserUuid userId2)
    {
        if (userId1.Id.CompareTo(userId2.Id) > 0)
        {
            (userId1, userId2) = (userId2, userId1);
        }
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        const string query = "SELECT ChannelId FROM PrivateChannels WHERE UserId1 = @id1 AND UserId2 = @id2";
        using var command = new SqliteCommand(query, conn);
        command.Parameters.Add(new SqliteParameter("id1", userId1.ToString()));
        command.Parameters.Add(new SqliteParameter("id2", userId2.ToString()));
        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        var channelId = new ChannelId((string)reader["ChannelId"]);

        return GetChannel(channelId);
    }
}