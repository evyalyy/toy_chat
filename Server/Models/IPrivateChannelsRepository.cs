namespace Server.Models;

public interface IPrivateChannelsRepository
{
    Channel AddPrivateChannel(UserUuid userId1, UserUuid userId2);

    Channel? GetPrivateChannel(UserUuid userId1, UserUuid userId2);

    List<Channel> GetPrivateChannels(UserUuid userId);
}