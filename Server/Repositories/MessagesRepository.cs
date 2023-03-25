using Message = Server.Data.Message;

namespace Server.Repositories;

public class MessagesRepository : IMessagesRepository
{
    private readonly ChatDbContext _db;

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

        var data = new Message
            { ChannelId = channelId, Content = content, SentTs = timestamp, UserId = senderId };
        Models.Message.ValidateData(data);
        var entry = _db.Messages.Add(data);
        _db.SaveChanges();
        return entry.Entity.Id;
    }

    public IEnumerable<Models.Message> GetMessages(long channel, int fromId)
    {
        return _db.Messages
            .Where(message => message.ChannelId == channel && message.Id > fromId)
            .Select(message => new Models.Message(message)).ToList();
    }

    public Models.Message GetMessage(long channel, int messageId)
    {
        var data = _db.Messages
            .First(m => m.ChannelId == channel && m.Id == messageId);
        return new Models.Message(data);
    }
}