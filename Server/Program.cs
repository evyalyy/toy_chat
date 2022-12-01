using Microsoft.Data.Sqlite;
using Server.Migrations;
using Server.Models;
using Server.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sqliteConnectionString = new SqliteConnectionStringBuilder
{
    DataSource = "chat.db"
}.ToString();

var migrationRunner = new MigrationsRunner(sqliteConnectionString);
migrationRunner.Add(new Initial());
migrationRunner.Add(new AddSentTs());
migrationRunner.Add(new AddChannels());
migrationRunner.Add(new AddPrivateChannels());
migrationRunner.Add(new AddChannelIdToMessage());
migrationRunner.Up();

builder.Configuration["DbConnectionString"] = sqliteConnectionString;

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IMessagesRepository, MessagesRepository>();
builder.Services.AddScoped<IChannelsRepository, ChannelsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();