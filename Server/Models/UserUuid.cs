using Microsoft.EntityFrameworkCore;

namespace Server.Models;

[Owned]
public class UserUuid
{
    public Guid Id { get; set; }

    private UserUuid(Guid id)
    {
        Id = id;
    }

    public UserUuid(string id)
    {
        Id = Guid.Parse(id);
    }

    public static UserUuid New()
    {
        return new UserUuid(Guid.NewGuid());
    }

    public override string ToString()
    {
        return Id.ToString();
    }
}