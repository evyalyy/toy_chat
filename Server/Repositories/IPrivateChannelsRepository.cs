using Server.Models;

namespace Server.Repositories;

public interface IPrivateChannelsRepository
{
    PrivateChannel AddPrivateChannel(UserIdPair userIds);

    PrivateChannel GetPrivateChannel(UserIdPair userIds);

    PrivateChannel GetPrivateChannel(long channelId);

    bool HasPrivateChannel(UserIdPair userIds);

    List<PrivateChannel> GetPrivateChannels(long userId);
}