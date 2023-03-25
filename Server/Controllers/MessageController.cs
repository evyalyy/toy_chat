using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Protocol;
using Server.Repositories;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{
    private readonly IChannelsRepository _channels;
    private readonly IGroupsRepository _groups;
    private readonly IPrivateChannelsRepository _privateChannels;
    private readonly IUsersRepository _users;

    public MessageController(IChannelsRepository channels, IGroupsRepository groups,
        IPrivateChannelsRepository privateChannels, IUsersRepository users)
    {
        _channels = channels;
        _groups = groups;
        _privateChannels = privateChannels;
        _users = users;
    }

    [HttpGet("GetChannelMessages")]
    public ActionResult<List<MessageClient>> GetChannelMessages(long channelId, int lastMessageId = 0)
    {
        var channel = _channels.GetChannel(channelId);
        var messages = channel.GetMessages(lastMessageId);
        return messages.Select(message => message.GetForClient()).ToList();
    }

    [HttpGet("GetPreviews")]
    public ActionResult<List<ConversationPreview>> GetPreviews(long userId, int numChannels = 5)
    {
        var channels = _channels.GetUserChannelsSorted(userId, numChannels);
        var previews = new List<ConversationPreview>();
        foreach (var channel in channels)
        {
            var preview = new ConversationPreview
            {
                Type = channel.Type(),
                LastMessage = channel.GetLastMessage().GetForClient()
            };

            if (channel.Type() == ChannelType.GROUP)
            {
                var group = _groups.FindGroupByChannel(channel.Id());
                preview.Name = group.Name();
            }
            if (channel.Type() == ChannelType.PRIVATE)
            {
                var privateChannel = _privateChannels.GetPrivateChannel(channel.Id());
                var counterpart = privateChannel.GetCounterPart(userId);
                preview.Name = _users.GetUser(counterpart).Name();
            }

            previews.Add(preview);
        }

        return previews;
    }
}