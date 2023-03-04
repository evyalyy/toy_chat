using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Data;

public class PrivateChannel
{
    public long UserId1 { get; set; }

    public long UserId2 { get; set; }

    public DateTime CreatedAt { get; set; }

    public long ChannelId { get; set; }
}