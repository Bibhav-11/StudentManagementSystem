using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMSClient.Migrations
{
    public partial class DepartmentActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsActive",
                table: "Departments",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Departments");
        }
    }
}
