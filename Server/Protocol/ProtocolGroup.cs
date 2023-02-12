using Server.Models;

namespace Server.Protocol;

public class GroupMemberInfo
{
    public long GroupId { get; set; }

    public long UserId { get; set; }
}