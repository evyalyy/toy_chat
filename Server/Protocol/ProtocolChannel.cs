using Server.Models;

namespace Server.Protocol;


public class ChannelClient
{
    public long Id { get; set; }
    
    public int LastMessageId { get; set; }
    
    public DateTime LastMessageTs { get; set; }
}

public class SentMessageClient
{
    public long Id { get; set; }
    
    public int LastMessageId { get; set; }
}