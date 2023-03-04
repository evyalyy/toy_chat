using Microsoft.AspNetCore.Mvc;
using Server.Protocol;
using Server.Repositories;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupController : ControllerBase
{
    private readonly IGroupsRepository _groups;

    public GroupController(IGroupsRepository groups)
    {
        _groups = groups;
    }

    [HttpPost("PostGroupMessage")]
    public ActionResult<SentMessageClient> PostGroupMessage(long groupId, long senderUserId, string content)
    {
        var group = _groups.GetGroup(groupId);

        var lastMessageInfo = group.SendMessage(senderUserId, content);

        return lastMessageInfo.GetForClient();
    }

    [HttpPost("AddMember")]
    public void AddMember(long groupId, long userId)
    {
        var group = _groups.GetGroup(groupId);
        group.AddMember(userId);
    }

    [HttpPost("AddGroup")]
    public ActionResult<long> AddGroup(long ownerId, string name, string description)
    {
        var group = _groups.AddGroup(name, description);
        group.AddMember(ownerId);
        return group.Id();
    }

    [HttpPut("UpdateName")]
    public void UpdateName(long groupId, string name)
    {
        var group = _groups.GetGroup(groupId);
        group.SetName(name);
    }

    [HttpGet("GetMembers")]
    public ActionResult<List<GroupMemberInfo>> GetMembers(long groupId)
    {
        var group = _groups.GetGroup(groupId);
        return group.GetMembers()
            .Select(member => new GroupMemberInfo { GroupId = member.GroupId, UserId = member.UserId }).ToList();
    }
}