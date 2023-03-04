using Server.Data;
using Server.Models;
using Group = Server.Models.Group;

namespace Server.Repositories;

public class GroupsRepository : IGroupsRepository
{
    private readonly ChatDbContext _db;
    private readonly IChannelsRepository _channels;
    private readonly IUsersRepository _users;

    public GroupsRepository(ChatDbContext db, IChannelsRepository channels, IUsersRepository users)
    {
        _db = db;
        _channels = channels;
        _users = users;
    }

    public Group AddGroup(string name, string description)
    {
        var channel = _channels.AddChannel();
        var groupData = new Data.Group { ChannelId = channel.Id(), Description = description, Name = name };
        var entry = _db.Groups.Add(groupData);
        _db.SaveChanges();
        return new Group(entry.Entity, this, _channels, _users);
    }

    public Group GetGroup(long groupId)
    {
        var groupData = _db.Groups.Find(groupId);
        if (groupData is null)
        {
            throw new Exception($"Group {groupId} not found");
        }

        return new Group(groupData, this, _channels, _users);
    }

    public void AddMember(long groupId, long userId)
    {
        if (IsUserInGroup(groupId, userId))
        {
            return;
        }

        var memberData = new GroupMember { GroupId = groupId, UserId = userId, JoinedAt = DateTime.Now };
        _db.GroupMembers.Add(memberData);
        _db.SaveChanges();
    }

    public bool IsUserInGroup(long groupId, long userId)
    {
        return _db.GroupMembers.Find(groupId, userId) is not null;
    }

    public IEnumerable<GroupMember> GetMembers(long groupId)
    {
        return _db.GroupMembers.Where(member => member.GroupId == groupId).ToList();
    }

    public void SetGroupInfo(long groupId, string name, string description)
    {
        var data = _db.Groups.Find(groupId);
        if (data is null)
        {
            throw new Exception($"Group {groupId} not found");
        }

        data.Name = name;
        data.Description = description;
        _db.Groups.Update(data);
        
        _db.SaveChanges();
    }

    // public void RemoveMember(GroupId groupId, UserUuid userId);
}