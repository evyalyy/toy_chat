using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Protocol;
using Server.Repositories;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class ChannelController : ControllerBase
{
    private readonly IChannelsRepository _channels;

    public ChannelController(IChannelsRepository channels)
    {
        _channels = channels;
    }

    [HttpGet("MyPrivateChannels")]
    public ActionResult<List<ChannelClient>> MyPrivateChannels(long userId)
    {
        return new List<ChannelClient>();
    }

    [HttpPost("AddChannel")]
    public ActionResult<long> DebugAddChannel()
    {
        var ch = _channels.AddChannel();
        return ch.Id();
    }

    [HttpPost("PostMessage")]
    public ActionResult<SentMessageClient> DebugPostMessage(long channelId, long senderId, string content)
    {
        var channel = _channels.GetChannel(channelId);
        
        var sentMessage = channel.SendMessage(senderId, content);
        return sentMessage.GetForClient();
    }
}