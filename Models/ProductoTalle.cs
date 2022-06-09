using System.ComponentModel.DataAnnotations;

namespace LaTiendita.Models
{
    public class ProductoTalle
    {
        [Key]
        public int ProductoTalleId { get; set; }

        
        public int TalleId { get; set; }
        public Talle Talle { get; set; }

        public int ProductoId{ get; set; }

        public ProductoBis Producto   { get; set; }


        public int Cantidad { get; set; }

    }
}
