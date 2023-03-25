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

    public long GetCounterPart(long userId)
    {
        if (userId == _data.UserId1)
        {
            return _data.UserId2;
        }

        if (userId == _data.UserId2)
        {
            return _data.UserId1;
        }

        throw new Exception($"User {userId} not in private channel {_data.ChannelId}");
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