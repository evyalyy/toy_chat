using Server.Models;

namespace Server.Protocol;

public class GroupMemberInfo
{
    public GroupId GroupId { get; set; }

    public long UserId { get; set; }
}