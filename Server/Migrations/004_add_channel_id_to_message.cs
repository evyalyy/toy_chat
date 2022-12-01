using Server.Utils;

namespace Server.Migrations;

public class AddChannelIdToMessage: IMigration
{
    public string UpCommand()
    {
        return @"
        ALTER TABLE Messages ADD COLUMN ChannelId TEXT NOT NULL DEFAULT '';
        CREATE INDEX MESSAGE_CHANNEL ON Messages (ChannelId);
";
    }
}