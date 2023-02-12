using Server.Protocol;

namespace Server.Models;

public interface IGroupsRepository
{
    Group AddGroup(string name, string description);

    Group? GetGroup(long groupId);

    void AddMember(long groupId, long userId);

    bool IsUserInGroup(long groupId, long userId);

    List<GroupMemberInfo> GetMembers(long groupId);
    // void RemoveMember(long groupId, UserUuid userId);
}