using Microsoft.AspNetCore.Mvc;
using Server.Protocol;
using Server.Repositories;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly IChannelsRepository _channels;

    public MessageController(IChannelsRepository channels)
    {
        _channels = channels;
    }

    [HttpGet]
    public ActionResult<List<MessageClient>> Get(long channelId, int lastMessageId = 0)
    {
        var channel = _channels.GetChannel(channelId);
        var messages = channel.GetMessages(lastMessageId);
        return messages.Select(message => message.GetForClient()).ToList();
    }
}