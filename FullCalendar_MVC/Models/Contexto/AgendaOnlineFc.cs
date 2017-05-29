using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using FullCalendar_MVC.Models.Identity;
using FullCalendar_MVC.Models.Interfaces;
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

        public DbSet<EventosAuditoria> EnventoAuditoria { get; set; }

        public override int SaveChanges()
        {
            try
            {
                var currentTime = DateTime.Now;

                foreach (var entidade in ChangeTracker.Entries())
                {

                    var tipoTabelaAuditoria = entidade.Entity.GetType().GetInterfaces()[0].GenericTypeArguments[0];
                    var registroTabelaAuditoria = Activator.CreateInstance(tipoTabelaAuditoria);

                    // Isto aqui é lento, mas serve como exemplo. 
                    // Depois procure trocar por FastMember ou alguma outra estratégia de cópia.
                    foreach (var propriedade in entidade.Entity.GetType().GetProperties())
                    {
                        var propertyInfo = registroTabelaAuditoria.GetType()
                            .GetProperty(propriedade.Name);
                        if (propertyInfo != null)
                        {
                            var memberInfo = entidade.Entity.GetType().GetProperty(propriedade.Name);
                            if (memberInfo != null)
                                propertyInfo.SetValue(registroTabelaAuditoria,memberInfo.GetValue(entidade.Entity, null));
                        }
                    }

                    /* Salve aqui usuário e data */
                    this.Set(registroTabelaAuditoria.GetType()).Add(registroTabelaAuditoria);
                    if (entidade.State == EntityState.Added)
                    {

                        if (entidade.Property("DataCriacao") != null)
                        {
                            entidade.Property("DataCriacao").CurrentValue = currentTime;
                        }
                        if (entidade.Property("UsuarioCriacao") != null)
                        {
                            entidade.Property("UsuarioCriacao").CurrentValue = HttpContext.Current != null ? HttpContext.Current.User.Identity.Name : "Usuario";
                        }
                    }

                    if (entidade.State == EntityState.Modified)
                    {
                        entidade.Property("DataCriacao").IsModified = false;
                        entidade.Property("UsuarioCriacao").IsModified = false;

                        if (entidade.Property("UltimaModificacao") != null)
                        {
                            entidade.Property("UltimaModificacao").CurrentValue = currentTime;
                        }
                        if (entidade.Property("UsuarioModificacao") != null)
                        {
                            entidade.Property("UsuarioModificacao").CurrentValue = HttpContext.Current != null ? HttpContext.Current.User.Identity.Name : "Usuario";
                        }
                    }
                }

                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);

                var exceptionsMessage = string.Concat(ex.Message, "Os erros de validações são: ", fullErrorMessage);

                throw new DbEntityValidationException(exceptionsMessage, ex.EntityValidationErrors);
            }
        }
    }
}