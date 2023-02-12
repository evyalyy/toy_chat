namespace Server.Models;

public interface IMessagesRepository
{
    int AddMessage(long channel, long senderId, string content, DateTime timestamp);

    List<Message> GetMessages(long channel, int fromId);
}