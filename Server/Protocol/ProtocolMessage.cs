namespace Server.Protocol;

public class MessageClient
{
    public int Id { get; set; }

    public long ChannelId { get; set; }

    public string Content { get; set; }

    public long UserId { get; set; }

    public DateTime SentTs { get; set; }
}