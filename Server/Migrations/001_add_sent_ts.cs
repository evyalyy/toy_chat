using Server.Utils;

namespace Server.Migrations;

public class AddSentTs: IMigration
{
    public string UpCommand()
    {
        return @"
        ALTER TABLE Messages ADD COLUMN SentTs INTEGER NOT NULL DEFAULT 0;
";
    }
}