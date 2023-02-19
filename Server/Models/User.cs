using Server.Data;
using Server.Protocol;

namespace Server.Models;

public class User
{
    private readonly UserData _data;

    public User(UserData data)
    {
        if (data.Name.Length == 0)
        {
            throw new Exception("User name cannot be empty");
        }

        if (data.Password.Length == 0)
        {
            throw new Exception("Password cannot be empty");
        }

        if (data.PhoneNumber.Length == 0)
        {
            throw new Exception("Phone number cannot be empty");
        }

        if (!data.PhoneNumber.StartsWith('+'))
        {
            throw new Exception("Phone number must start with +");
        }

        _data = data;
    }

    public UserClient GetForClient()
    {
        return new UserClient { Id = _data.Id, Name = _data.Name, PhoneNumber = _data.PhoneNumber };
    }
}