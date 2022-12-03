using Microsoft.Data.Sqlite;

namespace Server.Models;

public class PrivateChannelsRepository : IPrivateChannelsRepository
{
    private readonly IChannelsRepository _channels;
    private readonly ILogger<PrivateChannelsRepository> _logger;
    private readonly string _connectionString;

    public PrivateChannelsRepository(
        IConfiguration configuration,
        ILogger<PrivateChannelsRepository> logger,
        IChannelsRepository channels)
    {
        _connectionString = configuration["DbConnectionString"];
        _logger = logger;
        _channels = channels;
    }

    private static (UserUuid, UserUuid) _sortUserIds(UserUuid userId1, UserUuid userId2)
    {
        return userId1.Id.CompareTo(userId2.Id) > 0 ? (userId2, userId1) : (userId1, userId2);
    }

    public Channel AddPrivateChannel(UserUuid userId1, UserUuid userId2)
    {
        var newChannel = _channels.AddChannel();

        (userId1, userId2) = _sortUserIds(userId1, userId2);

        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        const string query = @"
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

    public Channel? GetPrivateChannel(UserUuid userId1, UserUuid userId2)
    {
        (userId1, userId2) = _sortUserIds(userId1, userId2);
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

        return _channels.GetChannel(channelId);
    }

    public List<Channel> GetPrivateChannels(UserUuid userId)
    {
        var outChannels = new List<Channel>();
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        const string query = "SELECT ChannelId FROM PrivateChannels WHERE UserId1 = @id OR UserId2 = @id";
        using var command = new SqliteCommand(query, conn);
        command.Parameters.Add(new SqliteParameter("id", userId.ToString()));
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var channelId = new ChannelId((string)reader["ChannelId"]);
            var channel = _channels.GetChannel(channelId);
            if (channel is not null)
            {
                outChannels.Add(channel);
            }
            else
            {
                _logger.LogWarning("Private channel {ChannelId} not found in channels", channelId);
            }
        }

        return outChannels;
    }
}