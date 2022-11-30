using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly IUsersRepository _users;
    private readonly IMessagesRepository _messages;

    public MessageController(IUsersRepository users, IMessagesRepository messages)
    {
        _users = users;
        _messages = messages;
    }

    [HttpPost]
    public ActionResult<int> Post(string sender, string content)
    {
        UserUuid senderUuid;
        try
        {
            senderUuid = new UserUuid(sender);
        }
        catch (Exception err)
        {
            return BadRequest("Bad user id: " + err);
        }
        
        var user = _users.GetUser(senderUuid);
        if (user is null)
        {
            return NotFound($"User {senderUuid} not found");
        }

        return _messages.AddMessage(senderUuid, content, DateTime.Now);
    }

    [HttpGet]
    public ActionResult<List<Message>> Get(int lastMessageId = 0)
    {
        return _messages.GetMessages(lastMessageId);
    }
}