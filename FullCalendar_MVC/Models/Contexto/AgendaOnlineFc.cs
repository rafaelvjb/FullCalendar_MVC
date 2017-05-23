using System.Data.Entity;

namespace FullCalendar_MVC.Models.Contexto
{
    public class AgendaOnlineFc : DbContext
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
    }
}