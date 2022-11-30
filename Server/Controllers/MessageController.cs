using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly ChatDbContext _chatDbContext;

    public MessageController(ChatDbContext chatDbContext)
    {
        _chatDbContext = chatDbContext;
    }

    [HttpPost]
    public ActionResult<UInt64> Post(string sender, string content)
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
        
        var user = _chatDbContext.Users.Find(senderUuid);
        if (user is null)
        {
            return NotFound($"User {senderUuid} not found");
        }

        var entityEntry = _chatDbContext.Messages.Add(new Message { Content = content, UserId = senderUuid });
        _chatDbContext.SaveChanges();
        return entityEntry.Entity.Id;
    }

    [HttpGet]
    public ActionResult<List<Message>> Get(UInt64 lastMessageId = 0)
    {
        return _chatDbContext.Messages.Where(m => m.Id > lastMessageId).ToList();
    }
}