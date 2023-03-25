using Server.Data;
using Server.Repositories;

namespace Server.Models;

public class Group
{
    private readonly Data.Group _data;

    private readonly IGroupsRepository _groups;
    private readonly IChannelsRepository _channels;
    private readonly IUsersRepository _users;

    public Group(Data.Group data, IGroupsRepository groups, IChannelsRepository channels, IUsersRepository users)
    {
        _data = data;
        _groups = groups;
        _channels = channels;
        _users = users;
    }

    public long Id()
    {
        return _data.Id;
    }

    public SentMessage SendMessage(long userId, string content)
    {
        var channel = _channels.GetChannel(_data.ChannelId);
        if (!_users.HasUser(userId))
        {
            throw new Exception($"User {userId} not found");
        }

        if (!HasMember(userId))
        {
            throw new Exception($"User {userId} not in group {_data.Id}");
        }

        return channel.SendMessage(userId, content);
    }

    public IEnumerable<Message> GetMessages(int fromId)
    {
        var channel = _channels.GetChannel(_data.ChannelId);
        return channel.GetMessages(fromId);
    }

    public void AddMember(long userId)
    {
        if (!_users.HasUser(userId))
        {
            throw new Exception($"User {userId} not found");
        }

        if (!HasMember(userId))
        {
            _groups.AddMember(Id(), userId);
        }
    }

    public bool HasMember(long userId)
    {
        return _groups.IsUserInGroup(Id(), userId);
    }

    public IEnumerable<GroupMember> GetMembers()
    {
        return _groups.GetMembers(Id());
    }

    public void SetName(string name)
    {
        _groups.SetGroupInfo(Id(), name, _data.Description);
    }
}