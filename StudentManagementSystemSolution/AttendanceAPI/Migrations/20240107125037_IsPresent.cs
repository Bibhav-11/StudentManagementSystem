using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceAPI.Migrations
{
    /// <inheritdoc />
    public partial class IsPresent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsPresent",
                table: "AttendanceRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPresent",
                table: "AttendanceRecords");
        }
    }
}
