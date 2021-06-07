using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Initializedatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "account");

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "account",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "account",
                table: "Role",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 1, "Super Administrador", "admin" });

            migrationBuilder.InsertData(
                schema: "account",
                table: "Role",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 2, "Administrador de tienda y cliente", "store" });

            migrationBuilder.InsertData(
                schema: "account",
                table: "Role",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 3, "Cliente", "customer" });

            migrationBuilder.InsertData(
                schema: "account",
                table: "User",
                columns: new[] { "Id", "Email", "LastName", "Name", "Password", "Phone", "RoleId" },
                values: new object[] { new Guid("2301d97c-cc4e-48b5-8b9a-a191bd3a7f88"), "jdvanegas4@gmail.com", "Vanegas Rodriguez", "Juan David", "KB+JFudzjaeG9KQsmz3d1+9paVa1s83diUM2FtchFNM=", "3196780859", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                schema: "account",
                table: "User",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User",
                schema: "account");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "account");
        }
    }
}
