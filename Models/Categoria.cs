using System.ComponentModel.DataAnnotations;
namespace LaTiendita.Models

{
    public class Categoria
    {
        [Key]
        public int CategoriaId { get; set; }

        public String Nombre { get; set; }

        public ICollection<ProductoBis> Productos { get; set; }
}
}
