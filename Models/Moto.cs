using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MottuWebApplication.Models
{
    public class Moto
    {
        [Key]
        public int IdMoto { get; set; }

        [Required, StringLength(10)]
        public string NmPlaca { get; set; }

        [Required, StringLength(30)]
        public string StMoto { get; set; }

        public double KmRodado { get; set; }

        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }

        [ForeignKey("Modelo")]
        public int IdModelo { get; set; }

        [ForeignKey("FilialDepartamento")]
        public int IdFilialDepartamento { get; set; }

    }
}
