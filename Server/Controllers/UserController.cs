using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly ChatDbContext _db;

    public UserController(ILogger<UserController> logger, ChatDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpPost]
    public ActionResult<long> Post(string phoneNumber, string password, string name)
    {
        _logger.LogInformation("Received CreateUser with name {Name}", name);

        var user = new User { Name = name, PhoneNumber = phoneNumber, Password = password };
        var entry = _db.Users.Add(user);
        
        _db.SaveChanges();
        
        return entry.Entity.Id;
    }

    [HttpGet]
    public ActionResult<User> Get(long userId)
    {
        var user = _db.Users.Find(userId);
        if (user is null)
        {
            return NotFound($"User {userId} not found");
        }
        return user;
    }
}