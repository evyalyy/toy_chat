namespace Server.Models;

public class GroupId
{
    public Guid Id { get; }

    private GroupId(Guid id)
    {
        Id = id;
    }

    public GroupId(string id)
    {
        Id = Guid.Parse(id);
    }

    public static GroupId New()
    {
        return new GroupId(Guid.NewGuid());
    }

    public static GroupId Empty()
    {
        return new GroupId(Guid.Empty);
    }

    public override string ToString()
    {
        return Id.ToString();
    }
}