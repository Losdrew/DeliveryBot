using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryBot.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddNewRobotStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TYPE robot_status ADD VALUE 'waiting_for_cargo' AFTER 'idle'");
            migrationBuilder.Sql("ALTER TYPE robot_status ADD VALUE 'ready_for_pickup' AFTER 'delivering'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TYPE robot_status RENAME TO robot_status_old");
            migrationBuilder.Sql("CREATE TYPE robot_status AS ENUM ('inactive','idle','delivering','returning','maintenance','danger')");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Robots");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Robots",
                type: "robot_status",
                nullable: true);

            migrationBuilder.Sql("DROP TYPE robot_status_old;");
        }
    }
}
