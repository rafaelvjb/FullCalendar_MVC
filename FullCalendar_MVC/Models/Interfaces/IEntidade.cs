using System;

namespace FullCalendar_MVC.Models.Interfaces
{
    public interface IEntidade<TClasseAuditada>
        where TClasseAuditada: class
    {
        DateTime DataCriacao { get; set; }
        String UsuarioCriacao { get; set; }
        DateTime? UltimaModificacao { get; set; }
        String UsuarioModificacao { get; set; }
    }
}