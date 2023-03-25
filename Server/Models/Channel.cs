using Server.Repositories;

namespace Server.Models;

public class Channel
{
    private readonly Data.Channel _data;
    private readonly IMessagesRepository _messages;
    private readonly IChannelsRepository _channels;

    public long Id()
    {
        return _data.Id;
    }

    private static void ValidateData(Data.Channel data)
    {
        if (data.Id == 0)
        {
            throw new Exception("Channel id cannot be 0");
        }
    }

    public Channel(Data.Channel data, IMessagesRepository messages, IChannelsRepository channels)
    {
        ValidateData(data);
        _data = data;
        _messages = messages;
        _channels = channels;
    }

    public SentMessage SendMessage(long senderId, string content)
    {
        var now = DateTime.Now;
        var lastMessageId = _messages.AddMessage(Id(), senderId, content, now);
        _channels.UpdateChannel(Id(), lastMessageId, now);
        return new SentMessage { ChannelId = Id(), MessageId = lastMessageId };
    }

    public IEnumerable<Message> GetMessages(int lastId = 0)
    {
        return _messages.GetMessages(Id(), lastId);
    }

    public Message GetLastMessage()
    {
        return _messages.GetMessage(Id(), _data.LastMessageId);
    }
}