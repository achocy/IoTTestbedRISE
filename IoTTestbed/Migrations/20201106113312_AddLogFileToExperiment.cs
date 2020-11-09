using Microsoft.EntityFrameworkCore.Migrations;

namespace IoTTestbed.Migrations
{
    public partial class AddLogFileToExperiment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Log",
                table: "Experiment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Log",
                table: "Experiment");
        }
    }
}
