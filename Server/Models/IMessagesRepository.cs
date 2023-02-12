namespace Server.Models;

public interface IMessagesRepository
{
    int AddMessage(ChannelId channel, long senderId, string content, DateTime timestamp);

    List<Message> GetMessages(ChannelId channel, int fromId);
}