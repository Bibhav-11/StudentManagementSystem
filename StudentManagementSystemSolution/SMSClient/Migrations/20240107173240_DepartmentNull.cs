using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMSClient.Migrations
{
    public partial class DepartmentNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Departments_DepartmentId",
                table: "Classes");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Classes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Departments_DepartmentId",
                table: "Classes",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Departments_DepartmentId",
                table: "Classes");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Departments_DepartmentId",
                table: "Classes",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
