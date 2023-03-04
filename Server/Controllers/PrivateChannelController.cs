using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Models;
using Server.Protocol;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class PrivateChannelController : ControllerBase
{
    private ChatDbContext _db;

    public PrivateChannelController(ChatDbContext db)
    {
        _db = db;
    }

    [HttpPost("Private")]
    public ActionResult<SentMessageClient> SendMessage(long senderUserId, long privateChannelId, string content)
    {
        // var user = _db.Users.Find(senderUserId);
        // if (user is null)
        // {
        //     return NotFound($"User {senderUserId} not found");
        // }
        //
        // var channel = _db.PrivateChannels.Find(privateChannelId);
        // if (channel is null)
        // {
        //     return NotFound($"Private channel {privateChannelId} not found");
        // }

        return new SentMessageClient();
        //
        // var message = new MessageData
        //     { ChannelId = privateChannelId, Content = content, UserId = senderUserId, SentTs = DateTime.Now };
        // var entry = _db.Messages.Add(message);
        //
        // return new SentMessageClient { Id = channel.Id, LastMessageId = entry.Entity.Id };
    }

    [HttpGet]
    public ActionResult<List<MessageClient>> GetMessages(long privateChannelId, int lastMessageId = 0)
    {
        // var channel = _db.Channels.Find(privateChannelId);
        // if (channel is null)
        // {
        //     return NotFound($"Channel {privateChannelId} not found");
        // }

        return new List<MessageClient>();
    }
}