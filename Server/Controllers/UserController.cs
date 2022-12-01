using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUsersRepository _users;

    public UserController(ILogger<UserController> logger, IUsersRepository users)
    {
        _logger = logger;
        _users = users;
    }

    [HttpPost]
    public ActionResult<UserUuid> Post(string phoneNumber, string password, string name)
    {
        _logger.LogInformation("Received CreateUser with name {Name}", name);
        var id = UserUuid.New();

        var outId = _users.AddUser(id, phoneNumber, password, name);
        return outId;
    }

    [HttpGet]
    public ActionResult<User> Get(string userId)
    {
        var user = _users.GetUser(new UserUuid(userId));
        if (user is null)
        {
            return NotFound();
        }

        return user;
    }
}