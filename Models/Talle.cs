using System.ComponentModel.DataAnnotations;

namespace LaTiendita.Models
{
    public class Talle
    {
        public int Id { get; set; }

        [RegularExpression("S|M|L|XL|s|m|l|xl",ErrorMessage ="Talle inválido")]
        public string Nombre { get; set; }
    }
}
