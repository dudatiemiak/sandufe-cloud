using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MottuWebApplication.Models
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }

        [Required, StringLength(100)]
        public string NmCliente { get; set; }

        [Required, StringLength(14)]
        public string NrCpf { get; set; }

        [Required, StringLength(100), EmailAddress]
        public string NmEmail { get; set; }

        [ForeignKey("Logradouro")]
        public int IdLogradouro { get; set; }


    }
}
