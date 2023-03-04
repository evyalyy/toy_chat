using Server.Protocol;

namespace Server.Models;

public class SentMessage
{
    public long ChannelId { get; set; }
    
    public int MessageId { get; set; }

    public SentMessageClient GetForClient()
    {
        return new SentMessageClient { Id = ChannelId, LastMessageId = MessageId };
    }
}