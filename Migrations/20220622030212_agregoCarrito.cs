using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaTiendita.Migrations
{
    public partial class agregoCarrito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Talles_Producto_ProductoId",
                table: "Talles");

            migrationBuilder.DropIndex(
                name: "IX_Talles_ProductoId",
                table: "Talles");

            migrationBuilder.DropColumn(
                name: "ProductoId",
                table: "Talles");

            migrationBuilder.AddColumn<int>(
                name: "CarritoId",
                table: "Producto",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Carritos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    Cantidad = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaDeCreacion = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carritos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carritos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaDeCreacion",
                value: new DateTime(2022, 6, 22, 0, 2, 11, 832, DateTimeKind.Local).AddTicks(6403));

            migrationBuilder.CreateIndex(
                name: "IX_Producto_CarritoId",
                table: "Producto",
                column: "CarritoId");

            migrationBuilder.CreateIndex(
                name: "IX_Carritos_UsuarioId",
                table: "Carritos",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Producto_Carritos_CarritoId",
                table: "Producto",
                column: "CarritoId",
                principalTable: "Carritos",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producto_Carritos_CarritoId",
                table: "Producto");

            migrationBuilder.DropTable(
                name: "Carritos");

            migrationBuilder.DropIndex(
                name: "IX_Producto_CarritoId",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "CarritoId",
                table: "Producto");

            migrationBuilder.AddColumn<int>(
                name: "ProductoId",
                table: "Talles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaDeCreacion",
                value: new DateTime(2022, 6, 21, 22, 26, 53, 832, DateTimeKind.Local).AddTicks(2283));

            migrationBuilder.CreateIndex(
                name: "IX_Talles_ProductoId",
                table: "Talles",
                column: "ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Talles_Producto_ProductoId",
                table: "Talles",
                column: "ProductoId",
                principalTable: "Producto",
                principalColumn: "Id");
        }
    }
}
