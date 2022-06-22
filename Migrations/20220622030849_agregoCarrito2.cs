using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaTiendita.Migrations
{
    public partial class agregoCarrito2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producto_Carritos_CarritoId",
                table: "Producto");

            migrationBuilder.DropIndex(
                name: "IX_Producto_CarritoId",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "CarritoId",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "Carritos");

            migrationBuilder.AddColumn<int>(
                name: "CarritoId",
                table: "ProductoTalle",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaDeCreacion",
                value: new DateTime(2022, 6, 22, 0, 8, 49, 281, DateTimeKind.Local).AddTicks(3416));

            migrationBuilder.CreateIndex(
                name: "IX_ProductoTalle_CarritoId",
                table: "ProductoTalle",
                column: "CarritoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoTalle_Carritos_CarritoId",
                table: "ProductoTalle",
                column: "CarritoId",
                principalTable: "Carritos",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductoTalle_Carritos_CarritoId",
                table: "ProductoTalle");

            migrationBuilder.DropIndex(
                name: "IX_ProductoTalle_CarritoId",
                table: "ProductoTalle");

            migrationBuilder.DropColumn(
                name: "CarritoId",
                table: "ProductoTalle");

            migrationBuilder.AddColumn<int>(
                name: "CarritoId",
                table: "Producto",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Cantidad",
                table: "Carritos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Producto_Carritos_CarritoId",
                table: "Producto",
                column: "CarritoId",
                principalTable: "Carritos",
                principalColumn: "Id");
        }
    }
}
