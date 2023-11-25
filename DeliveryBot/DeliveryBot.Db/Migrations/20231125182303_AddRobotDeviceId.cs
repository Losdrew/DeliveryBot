using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryBot.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddRobotDeviceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceId",
                table: "Robots",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Robots");
        }
    }
}
