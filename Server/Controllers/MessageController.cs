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
    public ActionResult<SentMessageClient> PostPrivate(string senderUserId, string targetUserId, string content)
    {
        var senderUuid = new UserUuid(senderUserId);
        var targetUuid = new UserUuid(targetUserId);

        var user = _users.GetUser(senderUuid);
        if (user is null)
        {
            return NotFound($"User {senderUuid} not found");
        }
        
        var channel = _privateChannels.GetPrivateChannel(senderUuid, targetUuid)
                      ?? _privateChannels.AddPrivateChannel(senderUuid, targetUuid);

        var lastMessageId = channel.SendMessage(senderUuid, content);

        return new SentMessageClient{Id = channel.Id, LastMessageId = lastMessageId};
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