using Microsoft.EntityFrameworkCore.Migrations;

namespace IoTTestbed.Migrations
{
    public partial class SensorModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sensor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RasIp = table.Column<string>(nullable: true),
                    Availability = table.Column<bool>(nullable: false), 
                    Rime = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensor", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sensor");
        }
    }
}
