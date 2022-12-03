namespace Server.Models;

public interface IChannelsRepository
{
    Channel AddChannel();

    Channel? GetChannel(ChannelId channelId);

    void UpdateChannel(ChannelId channelId, int lastMessageId, DateTime lastMessageTs);
}