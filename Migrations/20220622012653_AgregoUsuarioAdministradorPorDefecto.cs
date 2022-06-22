using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaTiendita.Migrations
{
    public partial class AgregoUsuarioAdministradorPorDefecto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Email", "FechaDeCreacion", "Nombre", "Rol" },
                values: new object[] { 1, "admin@admin.com", new DateTime(2022, 6, 21, 22, 26, 53, 832, DateTimeKind.Local).AddTicks(2283), "Admin", 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
