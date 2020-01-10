using Microsoft.EntityFrameworkCore.Migrations;

namespace MilitaryScheduler.Data.Migrations
{
    public partial class UserRelatedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Events",
                newName: "UserName");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Events",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Events",
                newName: "Text");
        }
    }
}
