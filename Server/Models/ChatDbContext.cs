using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Models;

public class ChatDbContext : DbContext
{
    public DbSet<Data.User> Users { get; set; }
    public DbSet<Data.Message> Messages { get; set; }
    public DbSet<Data.Channel> Channels { get; set; }
    public DbSet<Data.Group> Groups { get; set; }
    public DbSet<Data.GroupMember> GroupMembers { get; set; }

    // TODO: refactor
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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GroupMember>().HasKey(u => new 
        { 
            u.GroupId, 
            u.UserId 
        });
    }
}