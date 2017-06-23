using System;
using System.Collections.Generic;

namespace FullCalendar_MVC.Models
{
    public class Convenio
    {
        public Guid ConvenioId { get; set; }

        public String Nome { get; set; }

        public virtual ICollection<Eventos> Eventos { get; set; }
    }
}