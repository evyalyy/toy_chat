using Server.Models;

namespace Server.Protocol;


public class ChannelClient
{
    public ChannelId Id { get; set; }
    
    public int LastMessageId { get; set; }
    
    public DateTime LastMessageTs { get; set; }
}

public class SentMessageClient
{
    public ChannelId Id { get; set; }
    
    public int LastMessageId { get; set; }
}