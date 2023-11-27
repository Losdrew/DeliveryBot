using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryBot.Db.Migrations
{
    /// <inheritdoc />
    public partial class RenameRobotStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TYPE robot_status RENAME VALUE 'charging' TO 'maintenance'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TYPE robot_status RENAME VALUE 'maintenance' TO 'charging'");
        }
    }
}
