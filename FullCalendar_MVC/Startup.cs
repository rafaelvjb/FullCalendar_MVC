using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(FullCalendar_MVC.Startup))]
namespace FullCalendar_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}