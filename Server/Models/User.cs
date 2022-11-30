using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

public class User
{
    public UserUuid Id { get; set; }

    public string PhoneNumber { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }
}
