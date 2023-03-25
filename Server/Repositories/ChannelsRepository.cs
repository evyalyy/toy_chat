using Server.Data;
using Channel = Server.Models.Channel;

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

    public Channel AddChannel(ChannelType channelType)
    {
        var data = new Data.Channel{Type = channelType};
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

    public Channel GetChannel(long channelId)
    {
        var data = _db.Channels.Find(channelId);
        if (data is null)
        {
            throw new Exception($"Channel {channelId} not found");
        }

        return new Channel(data, _messages, this);
    }

    public bool HasMember(long channelId, long userId)
    {
        return _db.ChannelMembers.FirstOrDefault(
                member => member.ChannelId == channelId && member.UserId == userId) is not null;
    }

    public void AddMember(long channelId, long userId)
    {
        _db.ChannelMembers.Add(new ChannelMember { ChannelId = channelId, UserId = userId });
        _db.SaveChanges();
    }

    public List<Channel> GetUserChannelsSorted(long userId, int numChannels)
    {
        var channelsRepo = this;
        return _db.ChannelMembers
            .Where(m => m.UserId == userId)
            .Join(
                _db.Channels,
                member => member.ChannelId,
                channel => channel.Id,
                (member, channel) => channel)
            .OrderBy(ch => ch.LastMessageTs)
            .Take(numChannels)
            .Select(channelData => new Channel(channelData, _messages, channelsRepo))
            .ToList();
    }
}