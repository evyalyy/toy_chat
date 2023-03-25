using Server.Data;

namespace Server.Protocol;

public class ConversationPreview
{
    public ChannelType Type { get; set; }
    
    public string Name { get; set; }

    public MessageClient LastMessage { get; set; }
}