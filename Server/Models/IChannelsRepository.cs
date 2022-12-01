namespace Server.Models;

public interface IChannelsRepository
{
    Channel AddChannel();

    Channel AddPrivateChannel(UserUuid userId1, UserUuid userId2);

    Channel? GetChannel(ChannelId channelId);

    void UpdateChannel(ChannelId channelId, int lastMessageId, DateTime lastMessageTs);

    Channel? GetPrivateChannel(UserUuid userId1, UserUuid userId2);
}