using System.ComponentModel.DataAnnotations;

namespace LaTiendita.Models
{
    public class Usuario
    {

        [Key]
        public int UsuarioId { get; set; }

        public String Email { get; set; }

        public String Nombre { get; set; }


    }
}
