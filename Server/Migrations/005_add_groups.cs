using Server.Utils;

namespace Server.Migrations;

public class AddGroups: IMigration
{
    public string UpCommand()
    {
        return @"
        CREATE TABLE Groups (
            Id TEXT PRIMARY KEY,
            Name TEXT NOT NULL,
            Description TEXT NOT NULL,
            ChannelId TEXT NOT NULL,
            FOREIGN KEY (ChannelId) REFERENCES Channels(Id)
        );

        CREATE TABLE GroupMembers (
            GroupId TEXT,
            UserId INTEGER,
            PRIMARY KEY (GroupId, UserId),
            FOREIGN KEY (GroupId) REFERENCES Groups(Id),
            FOREIGN KEY (UserId) REFERENCES Users(Id)
        );
";
    }
}