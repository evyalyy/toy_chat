using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Server.Models;

using Microsoft.EntityFrameworkCore;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Message> Messages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var converter = new ValueConverter<UserUuid, string>(
            v => v.ToString(),
            v => new UserUuid(v)
        );

        modelBuilder
            .Entity<User>()
            .Property(e => e.Id)
            .HasConversion(converter);

        modelBuilder
            .Entity<Message>()
            .Property(e => e.UserId)
            .HasConversion(converter);
    }
}