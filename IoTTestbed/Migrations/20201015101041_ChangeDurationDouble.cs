using Microsoft.EntityFrameworkCore.Migrations;

namespace IoTTestbed.Migrations
{
    public partial class ChangeDurationDouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Duration",
                table: "Experiment",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Duration",
                table: "Experiment",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
