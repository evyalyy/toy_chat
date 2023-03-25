using Server.Data;
using Server.Models;
using PrivateChannel = Server.Models.PrivateChannel;

namespace Server.Repositories;

public class PrivateChannelsRepository : IPrivateChannelsRepository
{
    private readonly ChatDbContext _db;
    private readonly IChannelsRepository _channels;
    private readonly IUsersRepository _users;

    public PrivateChannelsRepository(ChatDbContext db, IChannelsRepository channels, IUsersRepository users)
    {
        _db = db;
        _channels = channels;
        _users = users;
    }

    public PrivateChannel AddPrivateChannel(UserIdPair userIds)
    {
        if (!_users.HasUser(userIds.UserId1()))
        {
            throw new Exception($"User {userIds.UserId1()} not found");
        }
        if (!_users.HasUser(userIds.UserId2()))
        {
            throw new Exception($"User {userIds.UserId2()} not found");
        }

        var channel = _channels.AddChannel(ChannelType.PRIVATE);
        var data = new Data.PrivateChannel
        {
            ChannelId = channel.Id(),
            UserId1 = userIds.UserId1(),
            UserId2 = userIds.UserId2(),
            CreatedAt = DateTime.Now
        };
        var added = _db.PrivateChannels.Add(data);
        _db.SaveChanges();
        
        _channels.AddMember(channel.Id(), userIds.UserId1());
        _channels.AddMember(channel.Id(), userIds.UserId2());

        return new PrivateChannel(added.Entity, _channels);
    }

    public PrivateChannel GetPrivateChannel(UserIdPair userIds)
    {
        var data = _db.PrivateChannels.Find(userIds.UserId1(), userIds.UserId2());
        if (data is null)
        {
            throw new Exception($"Private channel for user {userIds.UserId1()} and {userIds.UserId2()}");
        }

        return new PrivateChannel(data, _channels);
    }

    public PrivateChannel GetPrivateChannel(long channelId)
    {
        var data = _db.PrivateChannels.First(ch => ch.ChannelId == channelId);
        return new PrivateChannel(data, _channels);
    }

    public bool HasPrivateChannel(UserIdPair userIds)
    {
        return _db.PrivateChannels.Find(userIds.UserId1(), userIds.UserId2()) is not null;
    }

    public List<PrivateChannel> GetPrivateChannels(long userId)
    {
        return _db.PrivateChannels.Where(ch => ch.UserId1 == userId || ch.UserId2 == userId)
            .Select(ch => new PrivateChannel(ch, _channels)).ToList();
    }
}