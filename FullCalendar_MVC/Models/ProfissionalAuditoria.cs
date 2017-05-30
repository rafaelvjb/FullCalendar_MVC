using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FullCalendar_MVC.Models
{
    public class ProfissionalAuditoria
    {
        [Key]
        public Guid ProfissionalId { get; set; }

        public String Nome { get; set; }

        public bool Ativo { get; set; }

        //Relacionamento com tabela eventos
        public virtual ICollection<Eventos> Eventos { get; set; }
    }
}