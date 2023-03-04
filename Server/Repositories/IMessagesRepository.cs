using Server.Models;

namespace Server.Repositories;

public interface IMessagesRepository
{
    int AddMessage(long channel, long senderId, string content, DateTime timestamp);

    List<Message> GetMessages(long channel, int fromId);
}