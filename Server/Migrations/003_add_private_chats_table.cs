using Server.Utils;

namespace Server.Migrations;

public class AddPrivateChannels: IMigration
{
    public string UpCommand()
    {
        return @"
        CREATE TABLE PrivateChannels (
            UserId1 INTEGER,
            UserId2 INTEGER,
            ChannelId TEXT,
            PRIMARY KEY (UserId1, UserId2),
            FOREIGN KEY (UserId1) REFERENCES Users(Id),
            FOREIGN KEY (UserId2) REFERENCES Users(Id),
            FOREIGN KEY (ChannelId) REFERENCES Channels(Id)
        );
";
    }
}