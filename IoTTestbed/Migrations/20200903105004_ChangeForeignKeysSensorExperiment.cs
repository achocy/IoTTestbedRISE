using Microsoft.EntityFrameworkCore.Migrations;

namespace IoTTestbed.Migrations
{
    public partial class ChangeForeignKeysSensorExperiment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorExperiment_Sensor_ExperimentId",
                table: "SensorExperiment");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorExperiment_Sensor_SensorId",
                table: "SensorExperiment",
                column: "SensorId",
                principalTable: "Sensor",
                principalColumn: "SensorId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorExperiment_Sensor_SensorId",
                table: "SensorExperiment");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorExperiment_Sensor_ExperimentId",
                table: "SensorExperiment",
                column: "ExperimentId",
                principalTable: "Sensor",
                principalColumn: "SensorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
