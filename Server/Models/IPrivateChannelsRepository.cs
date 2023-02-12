namespace Server.Models;

public interface IPrivateChannelsRepository
{
    Channel AddPrivateChannel(long userId1, long userId2);

    Channel? GetPrivateChannel(long userId1, long userId2);

    List<Channel> GetPrivateChannels(long userId);
}