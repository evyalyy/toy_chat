using Server.Protocol;

namespace Server.Models;

public class GroupPreviewInfo
{
    public long GroupId { get; set; }
    
    public string GroupName { get; set; }

    public MessageClient LastMessage { get; set; }
}