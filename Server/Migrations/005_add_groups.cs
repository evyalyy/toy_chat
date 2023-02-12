using Server.Utils;

namespace Server.Migrations;

public class AddGroups: IMigration
{
    public string UpCommand()
    {
        return @"
        CREATE TABLE Groups (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Description TEXT NOT NULL,
            ChannelId INTEGER NOT NULL,
            FOREIGN KEY (ChannelId) REFERENCES Channels(Id)
        );

        CREATE TABLE GroupMembers (
            GroupId INTEGER,
            UserId INTEGER,
            PRIMARY KEY (GroupId, UserId),
            FOREIGN KEY (GroupId) REFERENCES Groups(Id),
            FOREIGN KEY (UserId) REFERENCES Users(Id)
        );
";
    }
}