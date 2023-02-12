using Server.Utils;

namespace Server.Migrations;

public class AddChannels: IMigration
{
    public string UpCommand()
    {
        return @"
        CREATE TABLE Channels (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            LastMessageId INTEGER NOT NULL DEFAULT 0,
            LastMessageTs INTEGER NOT NULL DEFAULT 0
        );
";
    }
}