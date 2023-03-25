namespace Server.Data;

public class ChannelMember
{
    public long Id { get; set; }

    public long ChannelId { get; set; }
    public Channel Channel { get; set; }
    
    public long UserId { get; set; }
    public User User { get; set; }
}