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

    public Models.User GetUser(long id)
    {
        var userData = _db.Users.Find(id);
        if (userData is null)
        {
            throw new Exception($"User {id} not found");
        }
        return new Models.User(userData);
    }

    public bool HasUser(long id)
    {
        return _db.Users.Find(id) is not null;
    }
}