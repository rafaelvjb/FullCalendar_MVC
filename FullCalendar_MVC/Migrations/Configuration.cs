namespace FullCalendar_MVC.Migrations
{
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
      
        }
    }
}
