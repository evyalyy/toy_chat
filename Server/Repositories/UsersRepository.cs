using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly ChatDbContext _db;

    public UsersRepository(ChatDbContext db)
    {
        _db = db;
    }

    public long AddUser(string phoneNumber, string password, string name)
    {
        var data = new UserData { Name = name, Password = password, PhoneNumber = phoneNumber };
        User.ValidateData(data);
        var entry = _db.Users.Add(data);
        _db.SaveChanges();
        return entry.Entity.Id;
    }

    public User? GetUser(long id)
    {
        var userData = _db.Users.Find(id);
        return userData is null ? null : new User(userData);
    }
}