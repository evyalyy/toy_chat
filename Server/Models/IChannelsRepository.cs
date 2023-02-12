namespace Server.Models;

public interface IChannelsRepository
{
    Channel AddChannel();

    Channel? GetChannel(long channelId);

    void UpdateChannel(long channelId, int lastMessageId, DateTime lastMessageTs);
}