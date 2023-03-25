using Server.Repositories;

namespace Server.Models;

public class PrivateChannel
{
    private readonly Data.PrivateChannel _data;
    private readonly IChannelsRepository _channels;

    public PrivateChannel(Data.PrivateChannel data, IChannelsRepository channels)
    {
        _data = data;
        _channels = channels;
    }

    public SentMessage SendMessage(long userId, string content)
    {
        var channel = _channels.GetChannel(_data.ChannelId);
        return channel.SendMessage(userId, content);
    }

    public IEnumerable<Message> GetMessages(int fromId)
    {
        var channel = _channels.GetChannel(_data.ChannelId);
        return channel.GetMessages(fromId);
    }
}