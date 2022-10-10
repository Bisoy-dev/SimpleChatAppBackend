using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatAppAPI.Migrations
{
    public partial class ModifiedRequestModelAndAddNotificationNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Requests");
        }
    }
}
