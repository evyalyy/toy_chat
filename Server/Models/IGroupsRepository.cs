using Server.Protocol;

namespace Server.Models;

public interface IGroupsRepository
{
    Group AddGroup(string name, string description);

    Group? GetGroup(GroupId groupId);

    void AddMember(GroupId groupId, long userId);

    bool IsUserInGroup(GroupId groupId, long userId);

    List<GroupMemberInfo> GetMembers(GroupId groupId);
    // void RemoveMember(GroupId groupId, UserUuid userId);
}