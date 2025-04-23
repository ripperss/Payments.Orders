using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PayMent.Orders.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Mercchant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MerchantEntityId",
                table: "Orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "MerchantId",
                table: "Orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "merchantEntities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    WebSite = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_merchantEntities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_MerchantEntityId",
                table: "Orders",
                column: "MerchantEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_merchantEntities_MerchantEntityId",
                table: "Orders",
                column: "MerchantEntityId",
                principalTable: "merchantEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_merchantEntities_MerchantEntityId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "merchantEntities");

            migrationBuilder.DropIndex(
                name: "IX_Orders_MerchantEntityId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MerchantEntityId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "Orders");
        }
    }
}
