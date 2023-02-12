namespace Server.Models;

public interface IUsersRepository
{
    long AddUser(string phoneNumber, string password, string name);

    public User? GetUser(long id);
}