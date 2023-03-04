using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Protocol;
using Server.Repositories;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class PrivateChannelController : ControllerBase
{
    private readonly IPrivateChannelsRepository _privateChannels;
    
    public PrivateChannelController(IPrivateChannelsRepository privateChannels)
    {
        _privateChannels = privateChannels;
    }

    [HttpPost("PostPrivate")]
    public ActionResult<SentMessageClient> PostPrivate(long senderUserId, long targetUserId, string content)
    {
        var userIds = new UserIdPair(senderUserId, targetUserId);
        if (_privateChannels.HasPrivateChannel(userIds))
        {
            var existingChannel = _privateChannels.GetPrivateChannel(userIds);
            return existingChannel.SendMessage(senderUserId, content).GetForClient();
        }

        var newChannel = _privateChannels.AddPrivateChannel(userIds);
        return newChannel.SendMessage(senderUserId, content).GetForClient();
    }

    [HttpGet]
    public ActionResult<List<MessageClient>> GetMessages(long privateChannelId, int lastMessageId = 0)
    {
        var ch = _privateChannels.GetPrivateChannel(privateChannelId);

        return ch.GetMessages(lastMessageId).Select(m => m.GetForClient()).ToList();
    }
}