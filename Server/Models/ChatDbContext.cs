using Microsoft.EntityFrameworkCore;

namespace Server.Models;


public class ChatDbContext: DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Channel> Channels { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<PrivateChannel> PrivateChannels { get; set; }

    private readonly IConfiguration _configuration;

    public ChatDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    // // The following configures EF to create a Sqlite database file in the
    // // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(_configuration.GetConnectionString("ChatDatabase"));
}