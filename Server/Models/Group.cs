using Server.Protocol;

namespace Server.Models;

public class Group
{
    public long Id { get; }
    
    public string Name { get; private set; }
    
    public string Description { get; private set; }

    private readonly long _channelId;

    private readonly IChannelsRepository _channels;

    private readonly IGroupsRepository _groups;

    public Group(
        IChannelsRepository channels,
        IGroupsRepository groups,
        long id,
        string name,
        string description,
        long channelId)
    {
        Id = id;
        Name = name;
        Description = description;
        _channelId = channelId;
        _channels = channels;
        _groups = groups;
    }

    public Channel? GetChannel()
    {
        return _channels.GetChannel(_channelId);
    }

    public void AddMember(long userId)
    {
        _groups.AddMember(Id, userId);
    }

    public List<GroupMemberInfo> GetMembers()
    {
        return _groups.GetMembers(Id);
    }
}