using Server.Models;

namespace Server.Repositories;

public class ChannelsRepository : IChannelsRepository
{
    private readonly ChatDbContext _db;
    private readonly IMessagesRepository _messages;
    public ChannelsRepository(ChatDbContext db, IMessagesRepository messages)
    {
        _db = db;
        _messages = messages;
    }

    public Channel AddChannel()
    {
        var data = new Data.Channel();
        var entry = _db.Channels.Add(data);
        _db.SaveChanges();
        return new Channel(entry.Entity, _messages, this);
    }

    public void UpdateChannel(long channelId, int lastMessageId, DateTime lastMessageTs)
    {
        var channel = _db.Channels.Find(channelId);
        if (channel is null)
        {
            throw new Exception($"Channel {channelId} not found");
        }

        channel.LastMessageId = lastMessageId;
        channel.LastMessageTs = lastMessageTs;
        _db.Channels.Update(channel);
        _db.SaveChanges();
    }

    public Channel? GetChannel(long channelId)
    {
       var data = _db.Channels.Find(channelId);
       return data is null ? null : new Channel(data, _messages, this);
    }
}