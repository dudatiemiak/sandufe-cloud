using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MottuWebApplication.Models
{
    public class Logradouro
    {
        [Key]
        public int IdLogradouro { get; set; }

        [Required, StringLength(100)]
        public string NmLogradouro { get; set; }

        [Required, StringLength(10)]
        public string NrLogradouro { get; set; }

        [StringLength(100)]
        public string NmComplemento { get; set; }

        [ForeignKey("Bairro")]
        public int IdBairro { get; set; }

    }
}
