using Microsoft.EntityFrameworkCore.Migrations;

namespace IoTTestbed.Migrations
{
    public partial class ChangeIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorExperiment_Experiment_ExperimentId",
                table: "SensorExperiment");

            migrationBuilder.DropForeignKey(
                name: "FK_SensorExperiment_Sensor_ExperimentId",
                table: "SensorExperiment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sensor",
                table: "Sensor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Experiment",
                table: "Experiment");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Sensor");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Experiment");

            migrationBuilder.AddColumn<int>(
                name: "SensorId",
                table: "Sensor",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ExperimentId",
                table: "Experiment",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sensor",
                table: "Sensor",
                column: "SensorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Experiment",
                table: "Experiment",
                column: "ExperimentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorExperiment_Experiment_ExperimentId",
                table: "SensorExperiment",
                column: "ExperimentId",
                principalTable: "Experiment",
                principalColumn: "ExperimentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SensorExperiment_Sensor_ExperimentId",
                table: "SensorExperiment",
                column: "ExperimentId",
                principalTable: "Sensor",
                principalColumn: "SensorId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorExperiment_Experiment_ExperimentId",
                table: "SensorExperiment");

            migrationBuilder.DropForeignKey(
                name: "FK_SensorExperiment_Sensor_ExperimentId",
                table: "SensorExperiment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sensor",
                table: "Sensor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Experiment",
                table: "Experiment");

            migrationBuilder.DropColumn(
                name: "SensorId",
                table: "Sensor");

            migrationBuilder.DropColumn(
                name: "ExperimentId",
                table: "Experiment");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Sensor",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Experiment",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sensor",
                table: "Sensor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Experiment",
                table: "Experiment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorExperiment_Experiment_ExperimentId",
                table: "SensorExperiment",
                column: "ExperimentId",
                principalTable: "Experiment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SensorExperiment_Sensor_ExperimentId",
                table: "SensorExperiment",
                column: "ExperimentId",
                principalTable: "Sensor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
