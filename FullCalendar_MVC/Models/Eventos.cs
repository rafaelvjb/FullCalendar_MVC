using System;
using System.ComponentModel.DataAnnotations;

namespace FullCalendar_MVC.Models
{
    public class Eventos
    {
        [Key]
        public int ID { get; set; }
        public Guid ProfissionalId { get; set; }

        public string title { get; set; }
        public DateTime start { get; set; }
        public String HoraEvento { get; set; }
        public String DuracaoEvento { get; set; }
        public DateTime end { get; set; }
        public int StatusEnum { get; set; }

        //relacionamento com tabela profissional
        public virtual Profissional Profissional { get; set; }
    }
}