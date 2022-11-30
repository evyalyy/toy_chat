namespace Server.Models;

public interface IMessagesRepository
{
    int AddMessage(UserUuid senderId, string content, DateTime timestamp);

    List<Message> GetMessages(int fromId);
}