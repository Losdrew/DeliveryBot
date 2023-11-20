using DeliveryBot.Db.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryBot.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:order_statuses", "pending,delivering,pickup_available,delivered,cancelled")
                .Annotation("Npgsql:PostgresExtension:postgis", ",,")
                .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.AddColumn<OrderStatuses>(
                name: "Status",
                table: "Orders",
                type: "order_statuses",
                nullable: false,
                defaultValue: OrderStatuses.Pending);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,")
                .OldAnnotation("Npgsql:Enum:order_statuses", "pending,delivering,pickup_available,delivered,cancelled")
                .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,");
        }
    }
}
