using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullCalendar_MVC.Models
{
    [Table("EventoAuditoria")]
    public class EventoAuditoria
    {
        public int ID { get; set; }

        public String Titulo { get; set; }

        public DateTime DataAntiga { get; set; }

        public DateTime DataNova { get; set; }

        public String UsuarioModificacao { get; set; }
    }
}