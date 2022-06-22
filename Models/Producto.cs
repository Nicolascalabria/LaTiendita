
namespace LaTiendita.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public string Detalle { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
        public List<ProductoTalle> Talles { get; set; } = new List<ProductoTalle>();
    }
}
