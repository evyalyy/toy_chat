namespace Server.Data;

public class Message
{
    public int Id { get; set; }

    public long ChannelId { get; set; }
    public Channel Channel { get; set; }

    public string Content { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }

    public DateTime SentTs { get; set; }
}