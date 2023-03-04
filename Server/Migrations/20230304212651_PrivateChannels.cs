using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class PrivateChannels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PrivateChannels",
                table: "PrivateChannels");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PrivateChannels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrivateChannels",
                table: "PrivateChannels",
                columns: new[] { "UserId1", "UserId2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PrivateChannels",
                table: "PrivateChannels");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "PrivateChannels",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrivateChannels",
                table: "PrivateChannels",
                column: "Id");
        }
    }
}
