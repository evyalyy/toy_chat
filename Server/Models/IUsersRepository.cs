namespace Server.Models;

public interface IUsersRepository
{
    UserUuid AddUser(UserUuid id, string phoneNumber, string password, string name);

    public User? GetUser(UserUuid id);
}