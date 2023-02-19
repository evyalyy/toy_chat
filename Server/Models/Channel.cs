using Server.Data;

namespace Server.Models;

public class Channel
{
    public long Id { get; private set; }

    public int LastMessageId { get; private set; }

    public DateTime LastMessageTs { get; private set; }

    public virtual IEnumerable<MessageData> Messages { get; private set; }

    private ChatDbContext _db;

    public Channel(ChatDbContext db)
    {
        _db = db;
    }

    public void UpdateLastMessageInfo(int lastMessageId, DateTime lastMessageTs)
    {
        if (lastMessageId == 0)
        {
            throw new Exception("last message id cannot be 0");
        }

        LastMessageId = lastMessageId;
        LastMessageTs = lastMessageTs;
    }

    // public Channel(
    //     IMessagesRepository messages,
    //     IChannelsRepository channels,
    //     long id, int lastMessageId, DateTime lastMessageTs)
    // {
    //     _messages = messages;
    //     _channels = channels;
    //     Id = id;
    //     LastMessageId = lastMessageId;
    //     LastMessageTs = lastMessageTs;
    // }

    public int SendMessage(long sender, string content)
    {
        var now = DateTime.Now;
        var lastMessageId = _messages.AddMessage(Id, sender, content, now);
        _channels.UpdateChannel(Id, lastMessageId, now);
        LastMessageId = lastMessageId;
        LastMessageTs = now;
        return lastMessageId;
    }

    public List<Message> GetMessages(int lastId = 0)
    {
        return _messages.GetMessages(Id, lastId);
    }

    private readonly IMessagesRepository _messages;
    private readonly IChannelsRepository _channels;
}