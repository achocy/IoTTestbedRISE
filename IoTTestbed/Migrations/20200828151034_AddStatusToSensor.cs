using Microsoft.EntityFrameworkCore.Migrations;

namespace IoTTestbed.Migrations
{
    public partial class AddStatusToSensor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Sensor",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Sensor");
        }
    }
}
