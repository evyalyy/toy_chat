namespace Server.Data;

public class Channel
{
    public long Id { get; set; }

    public int LastMessageId { get; set; }

    public DateTime LastMessageTs { get; set; }

    public List<Message> Messages;
}