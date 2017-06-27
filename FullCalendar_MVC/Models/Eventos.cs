using System;
using System.ComponentModel.DataAnnotations;
using FullCalendar_MVC.Models.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FullCalendar_MVC.Models
{
    [JsonObject(IsReference = true)]
    public class Eventos : IEntidade<EventosAuditoria>
    {
        [Key]
        public int ID { get; set; }
        public Guid ProfissionalId { get; set; }
        public Guid ConvenioId { get; set; }

        [Required(ErrorMessage = "O Nome não pode ser em branco!")]
        [StringLength(50, ErrorMessage = "Maximo de 30 caracteres!")]
        public string title { get; set; }

        public DateTime start { get; set; }
        public String HoraEvento { get; set; }
        //public String DuracaoEvento { get; set; }
        public DateTime end { get; set; }

        public bool Consulta { get; set; }

        public bool Retorno { get; set; }

        public String Observacoes { get; set; }

        //relacionamnto com tabela convenio e profissional
        public virtual Convenio Convenio { get; set; }
        public virtual Profissional Profissional { get; set; }

        public DateTime DataCriacao { get; set; }
        public string UsuarioCriacao { get; set; }
        public DateTime? UltimaModificacao { get; set; }
        public string UsuarioModificacao { get; set; }
    }
}