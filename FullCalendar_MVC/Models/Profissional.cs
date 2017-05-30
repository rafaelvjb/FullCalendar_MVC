﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FullCalendar_MVC.Models.Interfaces;

namespace FullCalendar_MVC.Models
{
    [Table("Profissional")]
    public class Profissional : IEntidade<ProfissionalAuditoria>
    {
        [Key]
        public Guid ProfissionalId { get; set; }

        public String Nome { get; set; }

        public bool Ativo { get; set; }

        //Relacionamento com tabela eventos
        public virtual ICollection<Eventos> Eventos { get; set; }

        public DateTime DataCriacao { get; set; }
        public string UsuarioCriacao { get; set; }
        public DateTime? UltimaModificacao { get; set; }
        public string UsuarioModificacao { get; set; }
    }
}