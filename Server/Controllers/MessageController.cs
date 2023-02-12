using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Protocol;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly IUsersRepository _users;
    private readonly IChannelsRepository _channels;
    private readonly IPrivateChannelsRepository _privateChannels;

    public MessageController(
        IUsersRepository users,
        IChannelsRepository channels,
        IPrivateChannelsRepository privateChannels)
    {
        _users = users;
        _channels = channels;
        _privateChannels = privateChannels;
    }

    [HttpPost("Private")]
    public ActionResult<SentMessageClient> PostPrivate(long senderUserId, long targetUserId, string content)
    {
        var user = _users.GetUser(senderUserId);
        if (user is null)
        {
            return NotFound($"User {senderUserId} not found");
        }
        
        var channel = _privateChannels.GetPrivateChannel(senderUserId, targetUserId)
                      ?? _privateChannels.AddPrivateChannel(senderUserId, targetUserId);

        var lastMessageId = channel.SendMessage(senderUserId, content);

        return new SentMessageClient{Id = channel.Id, LastMessageId = lastMessageId};
    }

    [HttpGet]
    public ActionResult<List<Message>> Get(long channelId, int lastMessageId = 0)
    {
        var channel = _channels.GetChannel(channelId);
        if (channel is null)
        {
            return NotFound($"Channel {channelId} not found");
        }
        return channel.GetMessages(lastMessageId);
    }
}