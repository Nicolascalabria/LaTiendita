using System.ComponentModel.DataAnnotations;

namespace LaTiendita.Models
{
    public class Talle
    {
        public int Id { get; set; }

        [RegularExpression("XS|S|M|L|XL|s|m|l|xl|xs",ErrorMessage ="Talle inválido")]
        public string Nombre { get; set; }
    }
}
