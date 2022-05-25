using System.ComponentModel.DataAnnotations;
namespace LaTiendita.Models

{
    public class Categoria
    {
        [Key]
        public int CategoriaId { get; set; }

        public String Nombre { get; set; }

        public ICollection<Producto> Productos { get; set; }
}
}
