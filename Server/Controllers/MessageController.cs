using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly MessageContext _messageContext;
    public MessageController(MessageContext messageContext)
    {
        _messageContext = messageContext;
    }

    [HttpPost]
    public ActionResult<UInt64> Post(string content)
    {
        var entityEntry = _messageContext.Messages.Add(new Message { Content = content });
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