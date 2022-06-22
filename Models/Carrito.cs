namespace LaTiendita.Models
{
    public class Carrito
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public List<CarritoProducto> Productos { get; set; } = new List<CarritoProducto>();
        public DateTime FechaDeCreacion { get; set; } = DateTime.Now;
    }
}

