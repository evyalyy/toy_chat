using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class MessagesRepository: IMessagesRepository
{
    private ChatDbContext _db;

    public MessagesRepository(ChatDbContext db)
    {
        _db = db;
    }
    public int AddMessage(long channelId, long senderId, string content, DateTime timestamp)
    {
        var channel = _db.Channels.Find(channelId);
        if (channel is null)
        {
            throw new Exception($"Channel {channelId} does not exist");
        }

        var data = new MessageData { ChannelId = channelId, Content = content, SentTs = timestamp, UserId = senderId };
        Message.ValidateData(data);
        var entry = _db.Messages.Add(data);
        _db.SaveChanges();
        return entry.Entity.Id;
    }

    public List<Message> GetMessages(long channel, int fromId)
    {
        return new List<Message>();
    }
}