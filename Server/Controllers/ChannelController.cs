using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Protocol;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class ChannelController : ControllerBase
{
    private readonly IPrivateChannelsRepository _channels;

    public ChannelController(IPrivateChannelsRepository channels)
    {
        _channels = channels;
    }

    [HttpGet("MyPrivateChannels")]
    public ActionResult<List<ChannelClient>> MyPrivateChannels(string userIdStr)
    {
        var userId = new UserUuid(userIdStr);
        var channels = _channels.GetPrivateChannels(userId);

        return channels.Select(
            channel => new ChannelClient
            {
                Id = channel.Id,
                LastMessageId = channel.LastMessageId,
                LastMessageTs = channel.LastMessageTs
            }).ToList();
    }
}