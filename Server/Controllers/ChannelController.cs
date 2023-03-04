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
    public ActionResult<long> AddChannel()
    {
        var ch = _channels.AddChannel();
        return ch.Id();
    }

    [HttpPost("PostMessage")]
    public ActionResult<SentMessageClient> PostMessage(long channelId, long sender, string content)
    {
        var channel = _channels.GetChannel(channelId);
        if (channel is null)
        {
            return NotFound($"Channel {channelId} not found");
        }

        var sentMessage = channel.SendMessage(sender, content);
        return sentMessage.GetForClient();
    }
}