using LaTiendita.Models;
using LaTiendita.Models.Enums;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasData(new Usuario { Id = 1, Email="admin@admin.com", Nombre="Admin", Rol=Roles.Administrador });
        }

        public DbSet<LaTiendita.Models.CarritoProducto> CarritoProducto { get; set; }
    }
}
