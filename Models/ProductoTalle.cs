namespace LaTiendita.Models
{
    public class ProductoTalle
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
        public int TalleId { get; set; }
        public Talle Talle { get; set; }
        public int Cantidad { get; set; }
    }
}
