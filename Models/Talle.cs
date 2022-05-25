using System.ComponentModel.DataAnnotations;
namespace LaTiendita.Models
{
    public class Talle
    {
        [Key]
        public int TalleId { get; set; }

        public String Nombre { get; set; }

    }
}
