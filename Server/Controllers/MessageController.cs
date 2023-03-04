using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Protocol;
using Server.Repositories;

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
        
        var channel = _privateChannels.GetPrivateChannel(senderUserId, targetUserId)
                      ?? _privateChannels.AddPrivateChannel(senderUserId, targetUserId);

        var lastMessageInfo = channel.SendMessage(senderUserId, content);

        return lastMessageInfo.GetForClient();
    }

    [HttpGet]
    public ActionResult<List<MessageClient>> Get(long channelId, int lastMessageId = 0)
    {
        var channel = _channels.GetChannel(channelId);
        if (channel is null)
        {
            return NotFound($"Channel {channelId} not found");
        }

        var messages = channel.GetMessages(lastMessageId);
        return messages.Select(message => message.GetForClient()).ToList();
    }
}