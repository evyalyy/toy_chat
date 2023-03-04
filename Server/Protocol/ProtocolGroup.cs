using Server.Models;

namespace Server.Protocol;

public class Group
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
}

public class GroupMemberInfo
{
    public long GroupId { get; set; }

    public long UserId { get; set; }
}