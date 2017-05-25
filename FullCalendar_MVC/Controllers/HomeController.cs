using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FullCalendar_MVC.Models;
using FullCalendar_MVC.Models.Contexto;
using FullCalendar_MVC.Models.ViewModels;

namespace FullCalendar_MVC.Controllers
{
    public class HomeController : Contoller
    {

        public ActionResult Index()
        {
            ViewBag.Profissionais = Db.Profissionais.ToList();
            return View();
        }

        public ActionResult Teste()
        {
            ViewBag.Profissionais = Db.Profissionais.ToList();
            return View();
        }

        public JsonResult Eventos(string start, string end, string usuarioId)
        {

            //var lista = Db.Eventos.ToList();
            var db = new AgendaOnlineFc(); // Isso deveria uma variável do controller
            var dtInicial = Convert.ToDateTime(start).Date;
            var dtfinal = Convert.ToDateTime(end).Date;
            IQueryable<Eventos> queryable = Db.Eventos;
            if (!string.IsNullOrEmpty(usuarioId))
            {
                var id = Guid.Parse(usuarioId);
                queryable = queryable.Where(d => d.ProfissionalId == id);
            }

            var lista = queryable.Where(d => d.end < dtfinal && d.start > dtInicial).ToList();

            var listaConvertida = new List<Eventos>();
            var datafim = new TimeSpan(4, 0, 0); // Não precisa criar isso uma vez por loop
            foreach (var item in lista)
            {
                var evento = new Eventos
                {
                    ID = item.ID,
                    title = item.title,
                    start = Convert.ToDateTime(item.start.Subtract(datafim)),
                    end = Convert.ToDateTime(item.end.Subtract(datafim))
                };

                listaConvertida.Add(evento);
            }
            return Json(listaConvertida, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult Eventos(string start, string end, string usuarioId)
        //{
        //    var id = Guid.Parse(usuarioId);
        //    var db = new AgendaOnlineFc();
        //    var dtInicial = Convert.ToDateTime(start).Date;
        //    var dtfinal = Convert.ToDateTime(end).Date;
        //    var lista = db.Eventos
        //        .Where(d => d.end < dtfinal && d.start > dtInicial && d.ProfissionalId == id)
        //        .ToList();
        //    var listaConvertida = new List<Eventos>();

        //    foreach (var item in lista)
        //    {
        //        var evento = new Eventos();
        //        evento.ID = item.ID;
        //        evento.title = item.title;
        //        var datafim = new TimeSpan(4, 0, 0);
        //        evento.start = Convert.ToDateTime(item.start.Subtract(datafim));/*TimeZone.CurrentTimeZone.GetUtcOffset(item.start);*///Convert.ToDateTime(item.start.ToString("dd/MM/yyyy HH:mm:ss"));
        //        evento.end = Convert.ToDateTime(item.end.Subtract(datafim));
        //        listaConvertida.Add(evento);
        //    }
        //    return Json(listaConvertida, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult AtualizarEvento(int id, string NewEventStart, string NewEventEnd)
        {
            DiaryEvent.AtualizarEventoDiario(id, NewEventStart, NewEventEnd);
            return RedirectToAction("/Home/Index");
        }

        public ActionResult DeletaEvento(int id)
        {
            var evento = Db.Eventos.FirstOrDefault(e => e.ID == id);
            if (evento != null) Db.Eventos.Remove(evento);
            Db.SaveChanges();
            return RedirectToAction("/Home/Index");
        }


        public ActionResult SaveEvent(EventoViewModel eventos)
        {
            DiaryEvent.CriaNovoEvento(eventos);
            return RedirectToAction("Index", "Home");
            //return true;
            //return DiaryEvent.CriaNovoEvento(eventos);
        }
        //public bool SaveEvent(string titulo, string dataEvento , string horaEvento, string duracaoEvento, string profissional)
        //{
        //    return DiaryEvent.CriaNovoEvento(titulo, dataEvento, horaEvento, duracaoEvento, profissional);
        //}

        //public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        //{
        //    // Unix timestamp is seconds past epoch
        //    System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        //    dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        //    return dtDateTime;
        //}




        public JsonResult GetDiarySummary(string start, string end)
        {
            //var ApptListForDate; //DiaryEvent.LoadAppointmentSummaryInDateRange(start, end);
            //var eventList = from e in ApptListForDate
            //                select new
            //                {
            //                    id = e.ID,
            //                    title = e.Title,
            //                    start = e.StartDateString,
            //                    end = e.EndDateString,
            //                    someKey = e.SomeImportantKeyID,
            //                    allDay = false
            //                };
            //var rows = eventList.ToArray();
            var lista = new List<Eventos>();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDiaryEvents(string start, string end, Guid usuarioId)
        {
            //var ApptListForDate = DiaryEvent.LoadAllAppointmentsInDateRange(start, end);
            //var eventList = from e in ApptListForDate
            //                select new
            //                {
            //                    id = e.ID,
            //                    title = e.Title,
            //                    start = e.StartDateString,
            //                    end = e.EndDateString,
            //                    color = e.StatusColor,
            //                    className = e.ClassName,
            //                    someKey = e.SomeImportantKeyID,
            //                    allDay = false
            //                };
            //var rows = eventList.ToArray();
            //return Json(rows, JsonRequestBehavior.AllowGet);
            var lista = new List<Eventos>();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }
    }
}
