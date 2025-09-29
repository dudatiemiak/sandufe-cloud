using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MottuWebApplication.Models
{
    public class Funcionario
    {
        [Key]
        public int IdFuncionario { get; set; }

        [Required, StringLength(100)]
        public string NmFuncionario { get; set; }

        [Required, StringLength(50)]
        public string NmCargo { get; set; }

        [Required, StringLength(100), EmailAddress]
        public string NmEmailCorporativo { get; set; }

        [Required, StringLength(255)]
        public string NmSenha { get; set; }

        [ForeignKey("Filial")]
        public int IdFilial { get; set; }
    }
}
