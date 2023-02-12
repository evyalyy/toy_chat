namespace Server.Models;

public class PrivateChannel
{
    public long Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public long ChannelId { get; set; }
    public Channel Channel { get; set; }

    public long UserId1 { get; set; }
    public User User1 { get; set; }
    
    public long UserId2 { get; set; }
    public User User2 { get; set; }

    public static (long, long) SortIds(long userId1, long userId2)
    {
        return userId1 < userId2 ? (userId1, userId2) : (userId2, userId1);
    }
}