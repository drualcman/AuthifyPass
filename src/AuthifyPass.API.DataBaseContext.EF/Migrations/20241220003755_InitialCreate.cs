using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthifyPass.API.DataBaseContext.EF.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Password = table.Column<string>(type: "VARCHAR(64)", nullable: true),
                    ClientId = table.Column<string>(type: "VARCHAR(32)", nullable: false),
                    SharedSecret = table.Column<string>(type: "VARCHAR(64)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.UniqueConstraint("AK_Clients_ClientId", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ClientId = table.Column<string>(type: "VARCHAR(32)", nullable: false),
                    UserId = table.Column<string>(type: "VARCHAR(64)", nullable: false),
                    ActiveSharedSecret = table.Column<string>(type: "VARCHAR(64)", nullable: false),
                    CreatetAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => new { x.ClientId, x.UserId, x.ActiveSharedSecret });
                    table.ForeignKey(
                        name: "FK_Users_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ClientId",
                table: "Clients",
                column: "ClientId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
