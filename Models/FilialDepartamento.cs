using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MottuWebApplication.Models
{
    public class FilialDepartamento
    {
        [Key]
        public int IdFilialDepartamento { get; set; }

        [ForeignKey("Filial")]
        public int IdFilial { get; set; }

        [ForeignKey("Departamento")]
        public int IdDepartamento { get; set; }

        public DateTime DtEntrada { get; set; }
        public DateTime? DtSaida { get; set; }
    }
}
