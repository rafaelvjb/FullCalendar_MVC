using FullCalendar_MVC.Models.Contexto;

namespace FullCalendar_MVC.Controllers
{
    public abstract class Contoller : System.Web.Mvc.Controller
    {
        public AgendaOnlineFc Db = new AgendaOnlineFc();
        //protected override void Initialize(RequestContext requestContext)
        //{
        //    base.Initialize(requestContext);
        //    JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        //    {
        //        DateFormatHandling = DateFormatHandling.IsoDateFormat,
        //        DateTimeZoneHandling = DateTimeZoneHandling.Local
        //    };
        //}
    }
}