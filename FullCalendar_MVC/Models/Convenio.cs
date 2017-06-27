using FullCalendar_MVC.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullCalendar_MVC.Models
{
    [Table("Convenio")]
    public class Convenio : IEntidade<ConvenioAuditoria>
    {
        [Key]
        public Guid ConvenioId { get; set; }
      
        public String Nome { get; set; }

       //public virtual Eventos Eventos { get; set; }
        public virtual ICollection<Eventos> Eventos { get; set; }

        public DateTime DataCriacao { get; set; }
        public string UsuarioCriacao { get; set; }
        public DateTime? UltimaModificacao { get; set; }
        public string UsuarioModificacao { get; set; }
    }
}