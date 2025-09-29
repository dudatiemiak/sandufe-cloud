using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MottuWebApplication.Models
{
    public class Filial
    {
        [Key]
        public int IdFilial { get; set; }

        [Required, StringLength(100)]
        public string NmFilial { get; set; }

        [ForeignKey("Logradouro")]
        public int IdLogradouro { get; set; }
    }
}
