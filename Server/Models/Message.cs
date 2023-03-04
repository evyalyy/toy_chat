using Server.Protocol;

namespace Server.Models;

public class Message
{
    private readonly Data.Message _data;

    public static void ValidateData(Data.Message data)
    {
        if (data.UserId == 0)
        {
            throw new Exception("UserId cannot be empty");
        }
    }

    public Message(Data.Message data)
    {
        ValidateData(data);
        _data = data;
    }

    public MessageClient GetForClient()
    {
        return new MessageClient
        {
            Id = _data.Id,
            ChannelId = _data.ChannelId,
            Content = _data.Content,
            SentTs = _data.SentTs,
            UserId = _data.UserId
        };
    }
}