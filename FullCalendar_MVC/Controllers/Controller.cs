using System.Web.Mvc;
using FullCalendar_MVC.Models.Contexto;
using FullCalendar_MVC.Models.Validacoes;

namespace FullCalendar_MVC.Controllers
{
    public abstract class Contoller : System.Web.Mvc.Controller
    {
        public AgendaOnlineFc Db = new AgendaOnlineFc();

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new CustomJsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
    }
}