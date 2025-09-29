using System.ComponentModel.DataAnnotations;

namespace MottuWebApplication.Models
{
    public class Departamento
    {
        [Key]
        public int IdDepartamento { get; set; }

        [Required, StringLength(50)]
        public string NmDepartamento { get; set; }

        [Required, StringLength(250)]
        public string DsDepartamento { get; set; }
    }
}
