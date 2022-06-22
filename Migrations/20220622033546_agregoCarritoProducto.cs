using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaTiendita.Migrations
{
    public partial class agregoCarritoProducto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "CarritoProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductoId = table.Column<int>(type: "INTEGER", nullable: false),
                    TalleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Cantidad = table.Column<int>(type: "INTEGER", nullable: false),
                    CarritoId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarritoProducto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarritoProducto_Carritos_CarritoId",
                        column: x => x.CarritoId,
                        principalTable: "Carritos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CarritoProducto_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarritoProducto_Talles_TalleId",
                        column: x => x.TalleId,
                        principalTable: "Talles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaDeCreacion",
                value: new DateTime(2022, 6, 22, 0, 35, 46, 666, DateTimeKind.Local).AddTicks(6070));

            migrationBuilder.CreateIndex(
                name: "IX_CarritoProducto_CarritoId",
                table: "CarritoProducto",
                column: "CarritoId");

            migrationBuilder.CreateIndex(
                name: "IX_CarritoProducto_ProductoId",
                table: "CarritoProducto",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_CarritoProducto_TalleId",
                table: "CarritoProducto",
                column: "TalleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarritoProducto");

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
    }
}
