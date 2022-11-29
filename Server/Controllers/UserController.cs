using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private UserContext _dbContext;

    public UserController(ILogger<UserController> logger, UserContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<ActionResult<User>> Post(string phoneNumber, string password, string name)
    {
        _logger.LogInformation("Received CreateUser with name {Name}", name);
        var newUser = new User { Id = Guid.NewGuid(), PhoneNumber = phoneNumber, Name = name, Password = password };
        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync();
        return newUser;
    }

    [HttpGet]
    public ActionResult<User> Get(string userId)
    {
        var user = _dbContext.Users.Find(Guid.Parse(userId));
        if (user is null)
        {
            return NotFound();
        }

        return user;
    }
}