using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FullCalendar_MVC.Models;
using FullCalendar_MVC.Models.ViewModels;

namespace FullCalendar_MVC.Controllers
{
    public class HomeController : Contoller
    {
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Profissionais = Db.Profissionais.ToList();
            return View();
        }

        public JsonResult Eventos(string start, string end, string usuarioId)
        {
            // var db = new AgendaOnlineFc(); // Isso deveria uma variável do controller
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
            var datafim = new TimeSpan(4, 0, 0);
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
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SalvaEvento(EventoViewModel eventos)
        {
            DiaryEvent.CriaNovoEvento(eventos);
            return RedirectToAction("Index", "Home");
            //return Json(new { message = "Criado com sucesso" }, JsonRequestBehavior.AllowGet);
        }

        #region Metodos não Utilizados

        //public JsonResult ObterEventosMes(string start, string end)
        //{

        //    var dataInicial = Guid.Parse(start);
        //    var datafinal = Guid.Parse(end);

        //    var lista = Db.Eventos
        //        .Where(d=> d.start >= dataInicial && d.end <= datafinal).To




        //    //var lista = new List<Eventos>();
        //    //return Json(lista, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetDiaryEvents(string start, string end)
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

        #endregion
    }
}
