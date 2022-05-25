using LaTiendita.Models;
using Microsoft.EntityFrameworkCore;

namespace LaTiendita.Stock

{ 

    public class BaseDeDatos : DbContext
    {
        

        public BaseDeDatos(DbContextOptions opciones) : base(opciones)
        {

        }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<LaTiendita.Models.Talle> Talles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

    }
}
