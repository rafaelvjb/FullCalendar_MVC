using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace FullCalendar_MVC.Models
{
    [Table("EventoAuditoria")]
    public class EventoAuditoria
    {
        [Key]
        public Guid  EventoAuditoriaId { get; set; }

        public int Identificacao { get; set; }

        public String Titulo { get; set; }

        public DateTime DataIniAntiga { get; set; }

        public DateTime DataFimAntiga { get; set; }

        public DateTime DataIniNova { get; set; }

        public DateTime DataFimNova { get; set; }

        public String UsuarioModificacao { get; set; }
    }
}