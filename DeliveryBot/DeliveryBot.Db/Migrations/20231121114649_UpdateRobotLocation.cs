using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace DeliveryBot.Db.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRobotLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Point>(
                name: "Location",
                table: "Robots",
                type: "geometry (point)",
                nullable: true,
                oldClrType: typeof(Point),
                oldType: "geometry",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Point>(
                name: "Location",
                table: "Robots",
                type: "geometry",
                nullable: true,
                oldClrType: typeof(Point),
                oldType: "geometry (point)",
                oldNullable: true);
        }
    }
}
