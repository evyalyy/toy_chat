using Server.Models;

namespace Server.Repositories;

public interface IUsersRepository
{
    long AddUser(string phoneNumber, string password, string name);

    public User GetUser(long id);

    public bool HasUser(long id);
}