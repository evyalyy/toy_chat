namespace Server.Models;

public class Message
{
    public int Id { get; set; }
    
    public ChannelId Channel { get; set; }

    public string Content { get; set; }

    public UserUuid UserId { get; set; }

    public DateTime SentTs { get; set; }
}