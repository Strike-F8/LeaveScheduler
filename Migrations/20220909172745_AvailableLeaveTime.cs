using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveScheduler.Migrations
{
    public partial class AvailableLeaveTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableLeaveTime",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableLeaveTime",
                table: "Employee");
        }
    }
}
