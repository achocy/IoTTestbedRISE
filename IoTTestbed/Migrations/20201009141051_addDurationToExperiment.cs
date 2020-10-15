using Microsoft.EntityFrameworkCore.Migrations;

namespace IoTTestbed.Migrations
{
    public partial class addDurationToExperiment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Experiment",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Experiment");
        }
    }
}
