using System.ComponentModel.DataAnnotations;

namespace MottuWebApplication.Models
{
    public class Modelo
    {
        [Key]
        public int IdModelo { get; set; }

        [Required, StringLength(50)]
        public string NmModelo { get; set; }
    }
}
