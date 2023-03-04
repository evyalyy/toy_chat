using Microsoft.AspNetCore.Mvc;
using Server.Protocol;
using Server.Repositories;

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
    public ActionResult<long> Post(string phoneNumber, string password, string name)
    {
        _logger.LogInformation("Received CreateUser with name {Name}", name);

        return _users.AddUser(phoneNumber, password, name);
    }

    [HttpGet]
    public ActionResult<UserClient> Get(long userId)
    {
        var user = _users.GetUser(userId);
        return user.GetForClient();
    }
}