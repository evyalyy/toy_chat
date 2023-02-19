namespace Server.Models;

public class User
{
    public long Id { get; private set; }

    public string PhoneNumber { get; private set; }

    public string Name { get; private set; }

    public string Password { get; private set; }

    private User()
    {
        // just for EF
    }

    public User(string name, string password, string phoneNumber)
    {
        if (name.Length == 0)
        {
            throw new Exception("User name cannot be empty");
        }

        if (password.Length == 0)
        {
            throw new Exception("Password cannot be empty");
        }

        if (phoneNumber.Length == 0)
        {
            throw new Exception("Phone number cannot be empty");
        }

        Id = 0;
        Name = name;
        Password = password;
        PhoneNumber = phoneNumber;
    }
}