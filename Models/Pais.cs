using System.ComponentModel.DataAnnotations;

namespace MottuWebApplication.Models
{
    public class Pais
    {
        [Key]
        public int IdPais { get; set; }

        [Required, StringLength(50)]
        public string NmPais { get; set; }
    }
}
