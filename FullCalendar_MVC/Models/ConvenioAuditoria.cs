using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FullCalendar_MVC.Models
{
    public class ConvenioAuditoria
    {
        [Key]
        public int ID { get; set; }

        public Guid ConvenioId { get; set; }

        public String Nome { get; set; }

        public virtual ICollection<Eventos> Eventos { get; set; }
    }
}