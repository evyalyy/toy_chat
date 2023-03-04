using Server.Models;
using User = Server.Data.User;

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
        var data = new User { Name = name, Password = password, PhoneNumber = phoneNumber };
        Models.User.ValidateData(data);
        var entry = _db.Users.Add(data);
        _db.SaveChanges();
        return entry.Entity.Id;
    }

    public Models.User? GetUser(long id)
    {
        var userData = _db.Users.Find(id);
        return userData is null ? null : new Models.User(userData);
    }
}