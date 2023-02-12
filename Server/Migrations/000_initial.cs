using Server.Utils;

namespace Server.Migrations;

public class Initial: IMigration
{
    public string UpCommand()
    {
        return @"
CREATE TABLE Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    PhoneNumber TEXT,
    Name TEXT,
    Password TEXT
);

CREATE TABLE Messages (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Content TEXT,
    UserId TEXT,
    FOREIGN KEY(UserId) REFERENCES Users(Id)
);
";
    }
}