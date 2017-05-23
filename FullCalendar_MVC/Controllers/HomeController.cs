using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FullCalendar_MVC.Models;
using FullCalendar_MVC.Models.Contexto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FullCalendar_MVC.Controllers
{
    public class HomeController : FullCalendar_MVC.Controllers.Contoller
    {
        public ActionResult Index()
        {
            return View();
        }

        //public string Init( string start, string end)
        //{
        //    //bool rslt = Utils.InitialiseDiary(start,end);
        //    //return rslt.ToString();
        //}

        public JsonResult Eventos(string start, string end)
        {
            var db = new AgendaOnlineFc();
            var dtInicial = Convert.ToDateTime(start).Date;
            var dtfinal = Convert.ToDateTime(end).Date;
            //var lista = db.Eventos
            //    .Where(d => d.end < dtfinal && d.start > dtInicial)
            //    .Select(e => new {
            //        ID = e.ID,
            //        title = e.title,
            //        start = e.start.ToString("dd/MM/yyyy HH:mm:ss"),
            //        end = e.end.Value.ToString("dd/MM/yyyy HH:mm:ss"),
            //        StatusEnum = e.StatusEnum
            //    })
            //    .ToList();

            // var db = new agendaonlinefc();
            //ar dtinicial = unixtimestamptodatetime(double.parse(start)); //convert.todatetime(start).date;//convert.todatetime(start).date;
            //var dtfinal = unixtimestamptodatetime(double.parse(start));//convert.todatetime(end).date;
            var lista = db.Eventos
                .Where(d => d.end < dtfinal && d.start > dtInicial)
                .ToList();
            var listaConvertida = new List<Eventos>();

            foreach (var item in lista)
            {
                var evento = new Eventos();
                evento.ID = item.ID;
                evento.title = item.title;
                var datafim = new TimeSpan(4, 0, 0);
                evento.start = Convert.ToDateTime(item.start.Subtract(datafim));/*TimeZone.CurrentTimeZone.GetUtcOffset(item.start);*///Convert.ToDateTime(item.start.ToString("dd/MM/yyyy HH:mm:ss"));
                evento.end = Convert.ToDateTime(item.end.Subtract(datafim));
                listaConvertida.Add(evento);
            }


            return Json(listaConvertida, JsonRequestBehavior.AllowGet);
        }
      
        public ActionResult AtualizarEvento(int id, string NewEventStart, string NewEventEnd)
        {
            DiaryEvent.AtualizarEventoDiario(id, NewEventStart, NewEventEnd);
            return RedirectToAction("/Home/Index");
        }

        public bool SaveEvent(string titulo, string novaDataEvento, string novaHoraEvento, string novoDuracaoEvento)
        {
            return DiaryEvent.CriaNovoEvento(titulo, novaDataEvento, novaHoraEvento, novoDuracaoEvento);
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }




        //public JsonResult GetDiarySummary(double start, double end)
        //{
        //    var ApptListForDate = DiaryEvent.LoadAppointmentSummaryInDateRange(start, end);
        //    var eventList = from e in ApptListForDate
        //                    select new
        //                    {
        //                        id = e.ID,
        //                        title = e.Title,
        //                        start = e.StartDateString,
        //                        end = e.EndDateString,
        //                        someKey = e.SomeImportantKeyID,
        //                        allDay = false
        //                    };
        //    var rows = eventList.ToArray();
        //    return Json(rows, JsonRequestBehavior.AllowGet);        
        //}

        //public JsonResult GetDiaryEvents(double start, double end)
        //{
        //    var ApptListForDate = DiaryEvent.LoadAllAppointmentsInDateRange(start, end);
        //    var eventList = from e in ApptListForDate
        //                    select new
        //                    {
        //                        id = e.ID,
        //                        title = e.Title,
        //                        start = e.StartDateString,
        //                        end = e.EndDateString,
        //                        color = e.StatusColor,
        //                        className = e.ClassName,
        //                        someKey = e.SomeImportantKeyID,
        //                        allDay = false
        //                    };
        //    var rows = eventList.ToArray();
        //    return Json(rows, JsonRequestBehavior.AllowGet);
        //}
    }
}
