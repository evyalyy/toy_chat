namespace Server.Data;

public enum ChannelType
{
    GROUP = 1,
    PRIVATE = 2
}

public class Channel
{
    public long Id { get; set; }

    public int LastMessageId { get; set; }

    public DateTime LastMessageTs { get; set; }
    
    public ChannelType Type { get; set; }

    public ICollection<ChannelMember> ChannelMembers { get; set; }
}