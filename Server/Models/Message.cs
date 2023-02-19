using Server.Data;

namespace Server.Models;

public class Message
{
    private MessageData _data;

    public Message(MessageData data)
    {
        _data = data;
    }
}