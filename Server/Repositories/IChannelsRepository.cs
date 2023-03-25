using Server.Data;
using Channel = Server.Models.Channel;

namespace Server.Repositories;

public interface IChannelsRepository
{
    Channel AddChannel(ChannelType channelType);

    Channel GetChannel(long channelId);

    void UpdateChannel(long channelId, int lastMessageId, DateTime lastMessageTs);

    bool HasMember(long channelId, long userId);

    void AddMember(long channelId, long userId);

    List<Channel> GetUserChannelsSorted(long userId, int numChannels);
}