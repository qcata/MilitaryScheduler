using Microsoft.EntityFrameworkCore.Migrations;

namespace MilitaryScheduler.Data.Migrations
{
    public partial class AddSysAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSystemAdmin",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSystemAdmin",
                table: "AspNetUsers");
        }
    }
}
