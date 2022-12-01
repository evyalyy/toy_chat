using Microsoft.AspNetCore.Mvc;
using Server.Models;

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

    [HttpPost]
    public ActionResult<Channel> PrivateChannel(string id1, string id2)
    {
        var userId1 = new UserUuid(id1);
        var userId2 = new UserUuid(id2);

        var channel = _channels.AddPrivateChannel(userId1, userId2);

        return channel;
    }
}