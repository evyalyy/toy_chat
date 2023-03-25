using Server.Models;

namespace Server.Repositories;

public interface IMessagesRepository
{
    int AddMessage(long channel, long senderId, string content, DateTime timestamp);

    IEnumerable<Message> GetMessages(long channel, int fromId);

    Message GetMessage(long channel, int messageId);
}