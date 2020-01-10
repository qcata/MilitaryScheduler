using Microsoft.EntityFrameworkCore.Migrations;

namespace MilitaryScheduler.Data.Migrations
{
    public partial class RevertedUserName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Events",
                newName: "Text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Events",
                newName: "UserName");
        }
    }
}
