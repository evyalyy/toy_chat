namespace Server.Models;

public class ChannelId
{
    public Guid Id { get; set; }

    private ChannelId(Guid id)
    {
        Id = id;
    }

    public ChannelId(string id)
    {
        Id = Guid.Parse(id);
    }

    public static ChannelId New()
    {
        return new ChannelId(Guid.NewGuid());
    }

    public override string ToString()
    {
        return Id.ToString();
    }
}