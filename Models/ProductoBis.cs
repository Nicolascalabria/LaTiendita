using System.ComponentModel.DataAnnotations;

namespace LaTiendita.Models
{
    public class ProductoBis
    {

        [Key]
        public int ProductoId { get; set; }

        public String Nombre { get; set; }


        public double Precio { get; set; }
        public String Detalle { get; set; }


        public int CategoriaId { get; set; }

        public Categoria Categoria { get; set; }

        public ICollection<ProductoTalle> ProductoTalle { get; set; } = new List<ProductoTalle>();

    }
}
