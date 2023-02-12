namespace Server.Models;

public class Channel
{
    public Channel(
        IMessagesRepository messages,
        IChannelsRepository channels,
        ChannelId id, int lastMessageId, DateTime lastMessageTs)
    {
        _messages = messages;
        _channels = channels;
        Id = id;
        LastMessageId = lastMessageId;
        LastMessageTs = lastMessageTs;
    }

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
    public ChannelId Id { get; }

    public int LastMessageId { get; private set; }

    public DateTime LastMessageTs { get; private set; }
}