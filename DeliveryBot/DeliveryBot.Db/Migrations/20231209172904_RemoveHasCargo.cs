using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryBot.Db.Migrations
{
    /// <inheritdoc />
    public partial class RemoveHasCargo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasCargo",
                table: "Robots");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasCargo",
                table: "Robots",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
