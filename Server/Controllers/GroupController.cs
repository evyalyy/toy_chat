using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Protocol;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupController : ControllerBase
{
    private readonly IUsersRepository _users;
    private readonly IGroupsRepository _groups;

    public GroupController(
        IUsersRepository users,
        IGroupsRepository groups)
    {
        _users = users;
        _groups = groups;
    }

    [HttpPost("PostGroupMessage")]
    public ActionResult<SentMessageClient> PostGroupMessage(string groupIdStr, string senderUserId, string content)
    {
        var senderUuid = new UserUuid(senderUserId);
        var groupId = new GroupId(groupIdStr);

        var user = _users.GetUser(senderUuid);
        if (user is null)
        {
            return NotFound($"User {senderUuid} not found");
        }

        var group = _groups.GetGroup(groupId);
        if (group is null)
        {
            return NotFound($"Group {groupId} does not exist");
        }

        var channel = group.GetChannel();
        if (channel is null)
        {
            throw new Exception($"Group {groupId} does not have channel. That should not happen");
        }

        var lastMessageId = channel.SendMessage(senderUuid, content);

        return new SentMessageClient { Id = channel.Id, LastMessageId = lastMessageId };
    }

    [HttpPost("AddMember")]
    public ActionResult AddMember(string groupIdStr, string userIdStr)
    {
        var userId = new UserUuid(userIdStr);
        var groupId = new GroupId(groupIdStr);

        var user = _users.GetUser(userId);
        if (user is null)
        {
            return NotFound($"User {userId} not found");
        }

        var group = _groups.GetGroup(groupId);
        if (group is null)
        {
            return NotFound($"Group {groupId} does not exist");
        }

        group.AddMember(userId);
        return Ok();
    }

    [HttpPost("AddGroup")]
    public ActionResult<Group> AddGroup(string owner, string name, string description)
    {
        var group = _groups.AddGroup(name, description);
        group.AddMember(new UserUuid(owner));
        return group;
    }

    [HttpGet("GetMembers")]
    public ActionResult<List<GroupMemberInfo>> GetMembers(string groupIdStr)
    {
        return _groups.GetMembers(new GroupId(groupIdStr));
    }
}