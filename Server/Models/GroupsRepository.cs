using Microsoft.Data.Sqlite;
using Server.Protocol;

namespace Server.Models;

public class GroupsRepository : IGroupsRepository
{
    private readonly string _connectionString;
    private readonly IChannelsRepository _channels;

    public GroupsRepository(IConfiguration configuration, IChannelsRepository channels)
    {
        _connectionString = configuration["DbConnectionString"];
        _channels = channels;
    }

    public Group AddGroup(string name, string description)
    {
        var groupId = GroupId.New();
        if (GetGroup(groupId) is not null)
        {
            throw new Exception($"Duplicate group id {groupId}");
        }

        var channel = _channels.AddChannel();

        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        const string query = @"
            INSERT INTO Groups (Id, Name, Description, ChannelId)
            VALUES (@id, @name, @description, @channelId)";
        using var command = new SqliteCommand(query, conn);
        command.Parameters.Add(new SqliteParameter("id", groupId.ToString()));
        command.Parameters.Add(new SqliteParameter("name", name));
        command.Parameters.Add(new SqliteParameter("description", description));
        command.Parameters.Add(new SqliteParameter("channelId", channel.Id.ToString()));
        var numRowsAffected = command.ExecuteNonQuery();
        if (numRowsAffected != 1)
        {
            throw new Exception($"Cannot create group {groupId}");
        }

        return new Group(
            _channels,
            this,
            groupId,
            name,
            description,
            channel.Id);
    }

    public Group? GetGroup(GroupId groupId)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        var query = @"
            SELECT Id, Name, Description, ChannelId FROM Groups
            WHERE Id = @id";
        using var command = new SqliteCommand(query, conn);
        command.Parameters.Add(new SqliteParameter("id", groupId.ToString()));
        var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        return new Group(
            _channels,
            this,
            groupId,
            (string)reader["Name"],
            (string)reader["Description"],
            new ChannelId((string)reader["ChannelId"]));
    }

    public void AddMember(GroupId groupId, UserUuid userId)
    {
        if (IsUserInGroup(groupId, userId))
        {
            return;
        }
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        const string query = @"
            INSERT INTO GroupMembers (GroupId, UserId)
            VALUES (@groupId, @userId)";
        using var command = new SqliteCommand(query, conn);
        command.Parameters.Add(new SqliteParameter("groupId", groupId.ToString()));
        command.Parameters.Add(new SqliteParameter("userId", userId.ToString()));

        var numRowsAffected = command.ExecuteNonQuery();
        if (numRowsAffected != 1)
        {
            throw new Exception($"Cannot add member {userId} to group {groupId}");
        }
    }

    public bool IsUserInGroup(GroupId groupId, UserUuid userId)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        const string query = @"
            SELECT 1 FROM GroupMembers
            WHERE GroupId = @groupId AND UserId = @userId";
        using var command = new SqliteCommand(query, conn);
        command.Parameters.Add(new SqliteParameter("groupId", groupId.ToString()));
        command.Parameters.Add(new SqliteParameter("userId", userId.ToString()));

        var reader = command.ExecuteReader();

        return reader.Read();
    }

    public List<GroupMemberInfo> GetMembers(GroupId groupId)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        const string query = @"
            SELECT UserId FROM GroupMembers
            WHERE GroupId = @groupId";
        using var command = new SqliteCommand(query, conn);
        command.Parameters.Add(new SqliteParameter("groupId", groupId.ToString()));

        var reader = command.ExecuteReader();

        var outMembersInfo = new List<GroupMemberInfo>();

        while (reader.Read())
        {
            outMembersInfo.Add(new GroupMemberInfo
            {
                GroupId = groupId,
                UserId = new UserUuid((string)reader["UserId"])
            });
        }

        return outMembersInfo;
    }

    // public void RemoveMember(GroupId groupId, UserUuid userId);
}