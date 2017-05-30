using System;
using System.Data.Entity;
using FullCalendar_MVC.Models.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FullCalendar_MVC.Models.Contexto
{
    public class AgendaOnlineFc : IdentityDbContext<Usuario, Grupo, Guid, UsuarioLogin, UsuarioGrupo, UsuarioIdentificacao>
    {
        public AgendaOnlineFc()
            : base("AgendaOnlineFc")
        {

        }

        public static AgendaOnlineFc Create()
        {
            return new AgendaOnlineFc();
        }

        public DbSet<Eventos> Eventos { get; set; }

        public DbSet<Profissional> Profissionais { get; set; }

        public DbSet<EventoAuditoria> EventoAuditoria { get; set; }
    }
}