using LaTiendita.Models;
using Microsoft.EntityFrameworkCore;

namespace LaTiendita.Stock
{ 
    public class BaseDeDatos : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ProductoTalle> ProductoTalle { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Talle> Talles { get; set; }
        public DbSet<Carrito> Carritos { get; set; }

        public BaseDeDatos(DbContextOptions opciones) : base(opciones)
        {
        }

        public DbSet<LaTiendita.Models.CarritoProducto> CarritoProducto { get; set; }
    }
}
