namespace FullCalendar_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<FullCalendar_MVC.Models.Contexto.AgendaOnlineFc>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Models.Contexto.AgendaOnlineFc context)
        {
            //context.Roles.AddOrUpdate(
            //  p => p.Name,
            //  new Models.Identity.Grupo
            //  {
            //      Id = Guid.NewGuid() ,Name = "Administradores"
            //  }
            //);
        }
    }
}
