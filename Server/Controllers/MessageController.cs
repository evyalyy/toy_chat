using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly MessageContext _messageContext;
    private readonly UserContext _userContext;

    public MessageController(MessageContext messageContext, UserContext userContext)
    {
        _messageContext = messageContext;
        _userContext = userContext;
    }

    [HttpPost]
    public ActionResult<UInt64> Post(string sender, string content)
    {
        Guid senderGuid;
        try
        {
            senderGuid = Guid.Parse(sender);
        }
        catch (Exception err)
        {
            return BadRequest("Bad user id: " + err);
        }
        
        var user = _userContext.Users.Find(senderGuid);
        if (user is null)
        {
            return NotFound($"User {senderGuid} not found");
        }

        var entityEntry = _messageContext.Messages.Add(new Message { Content = content, Sender = senderGuid });
        _messageContext.SaveChanges();
        return entityEntry.Entity.Id;
    }

    [HttpGet]
    public ActionResult<List<Message>> Get(UInt64 lastMessageId = 0)
    {
        var messages = _messageContext.Messages.Where(m => m.Id > lastMessageId);
        var outMessages = new List<Message>();
        foreach (var message in messages)
        {
            outMessages.Add(message);
        }

        return outMessages;
    }
}