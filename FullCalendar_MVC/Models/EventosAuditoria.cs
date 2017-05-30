using System;
using System.ComponentModel.DataAnnotations;

namespace FullCalendar_MVC.Models
{
    public class EventosAuditoria
    {
        [Key]
        public int ID { get; set; }
        public Guid ProfissionalId { get; set; }

        [Required(ErrorMessage = "O Nome não pode ser em branco!")]
        [StringLength(50, ErrorMessage = "Maximo de 30 caracteres!")]
        public string title { get; set; }

        public DateTime start { get; set; }
        public String HoraEvento { get; set; }
        public String DuracaoEvento { get; set; }
        public DateTime end { get; set; }
        public int StatusEnum { get; set; }
        public String className { get; set; }
        public String Observacoes { get; set; }

        public virtual Profissional Profissional { get; set; }
    }
}