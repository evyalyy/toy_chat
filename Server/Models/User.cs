using Server.Data;
using Server.Protocol;
using Server.Repositories;

namespace Server.Models;

public class User
{
    private readonly Data.User _data;

    public static void ValidateData(Data.User data)
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
    }

    public User(Data.User data)
    {
        ValidateData(data);

        _data = data;
    }

    public long Id()
    {
        return _data.Id;
    }

    public string Name()
    {
        return _data.Name;
    }

    public UserClient GetForClient()
    {
        return new UserClient { Id = _data.Id, Name = _data.Name, PhoneNumber = _data.PhoneNumber };
    }
}