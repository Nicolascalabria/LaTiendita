using LaTiendita.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaTiendita.Models
{
    public class Producto
    {
        [Key]
        public int ProductoId { get; set; }

        public String Nombre { get; set; }

      
        public double Precio { get; set; }
        public String Detalle { get; set; }

       
        public int CategoriaId { get; set; }

        
        public Categoria Categoria { get; set; }

    }
}
