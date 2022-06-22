using LaTiendita.Models.Enums;

namespace LaTiendita.Models
{
    public class Usuario
    {
       
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaDeCreacion { get; set; } = DateTime.Now;
        public Roles Rol { get; set; }
    }
}
