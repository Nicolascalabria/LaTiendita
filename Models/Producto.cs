using LaTiendita.Models;
using System.ComponentModel.DataAnnotations;

namespace LaTiendita.Models
{
    public class Producto
    {
        public int CodigoProducto { get; set; }
        
       
        public String Nombre { get; set; }

        public double Precio { get; set; }

        public String Detalle { get; set; }

    }
}
