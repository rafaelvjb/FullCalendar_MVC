using System.Web.Routing;
using Newtonsoft.Json;

namespace FullCalendar_MVC.Controllers
{
    public class Contoller : System.Web.Mvc.Controller
    {
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Local
            };
        }
    }
}