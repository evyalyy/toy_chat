using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly IUsersRepository _users;
    private readonly IChannelsRepository _channels;

    public MessageController(IUsersRepository users, IChannelsRepository channels)
    {
        _users = users;
        _channels = channels;
    }

    [HttpPost]
    public ActionResult<int> Post(string channelId, string sender, string content)
    {
        var senderUuid = new UserUuid(sender);

        var user = _users.GetUser(senderUuid);
        if (user is null)
        {
            return NotFound($"User {senderUuid} not found");
        }
        
        var channel = _channels.GetChannel(new ChannelId(channelId));

        if (channel is null)
        {
            return NotFound($"Channel {channelId} not found");
        }

        return channel.SendMessage(senderUuid, content);
    }

    [HttpGet]
    public ActionResult<List<Message>> Get(string channelId, int lastMessageId = 0)
    {
        var channel = _channels.GetChannel(new ChannelId(channelId));
        if (channel is null)
        {
            return NotFound($"Channel {channelId} not found");
        }
        return channel.GetMessages(lastMessageId);
    }
}