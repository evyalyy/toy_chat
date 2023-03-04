using Server.Data;
using Group = Server.Models.Group;

namespace Server.Repositories;

public interface IGroupsRepository
{
    Group AddGroup(string name, string description);

    Group GetGroup(long groupId);

    void AddMember(long groupId, long userId);

    bool IsUserInGroup(long groupId, long userId);

    IEnumerable<GroupMember> GetMembers(long groupId);

    void SetGroupInfo(long groupId, string name, string description);
    // void RemoveMember(long groupId, UserUuid userId);
}