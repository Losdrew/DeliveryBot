using DeliveryBot.Db.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryBot.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddRobotStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:order_status", "pending,delivering,pickup_available,delivered,cancelled")
                .Annotation("Npgsql:Enum:robot_status", "inactive,idle,delivering,returning,charging,danger")
                .Annotation("Npgsql:PostgresExtension:postgis", ",,")
                .OldAnnotation("Npgsql:Enum:order_status", "pending,delivering,pickup_available,delivered,cancelled")
                .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Robots");

            migrationBuilder.AddColumn<RobotStatus>(
                name: "Status",
                table: "Robots",
                type: "robot_status",
                nullable: false,
                defaultValue: RobotStatus.Inactive);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:order_status", "pending,delivering,pickup_available,delivered,cancelled")
                .Annotation("Npgsql:PostgresExtension:postgis", ",,")
                .OldAnnotation("Npgsql:Enum:order_status", "pending,delivering,pickup_available,delivered,cancelled")
                .OldAnnotation("Npgsql:Enum:robot_status", "inactive,idle,delivering,returning,charging,danger")
                .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Robots");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Robots",
                type: "text",
                nullable: true);
        }
    }
}
