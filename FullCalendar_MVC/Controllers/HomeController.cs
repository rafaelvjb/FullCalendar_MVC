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

        //lista todos os eventos
        public JsonResult Eventos(string start, string end, string usuarioId)
        {
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

        public ActionResult AtualizaEvento(EventoViewModel eventos)
        {
            var data = DateTime.Parse(eventos.DataEvento);
            var hora = eventos.HoraEvento.Split(':');
            data = data.AddHours(double.Parse(hora[0]));
            var start = data.AddMinutes(double.Parse(hora[1]));

            var datafim = DateTime.Parse(eventos.DataEvento);
            hora = eventos.DuracaoEvento.Split(':');
            datafim = datafim.AddHours(double.Parse(hora[0]));
            var end = datafim.AddMinutes(double.Parse(hora[1]));

            var ev = new Eventos()
            {
                ID = eventos.ID,
                title = eventos.Titulo,
                start = start,
                end = end,
                ProfissionalId = Guid.Parse(eventos.ProfissionalId),
                Observacoes = eventos.Observacoes
            };

            Db.Entry(ev).State = System.Data.Entity.EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        //Deleta Evento
        public ActionResult DeletaEvento(int? id)
        {
            var evento = Db.Eventos.FirstOrDefault(e => e.ID == id);
            if (evento != null)
            {
                Db.Eventos.Remove(evento);
            }
            Db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        //Salva o Evento
        public ActionResult SalvaEvento(EventoViewModel eventos)
        {
            var rec = new Eventos();
            rec.title = eventos.Titulo;
            var data = DateTime.Parse(eventos.DataEvento);
            var hora = eventos.HoraEvento.Split(':');
            data = data.AddHours(Convert.ToDouble(hora[0]));
            data = data.AddMinutes(Convert.ToDouble(hora[1]));
            rec.start = data;

            if (!String.IsNullOrEmpty(eventos.DuracaoEvento))
            {
                var duracao = int.Parse(eventos.DuracaoEvento);
                rec.end = rec.start.AddMinutes(duracao);
            }
            else
            {
                rec.end = rec.start.AddMinutes(30);
            }

            rec.ProfissionalId = Guid.Parse(eventos.ProfissionalId);
            Db.Eventos.Add(rec);
            Db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        // Atualiza a duração do evento
        public JsonResult AtualizaDuracao(int id, string NewEventStart, string NewEventEnd)
        {
            var evento = Db.Eventos.FirstOrDefault(e => e.ID == id);
            evento.ID = id;
            evento.start = Convert.ToDateTime(NewEventStart);
            evento.end = Convert.ToDateTime(NewEventEnd);
            Db.Entry(evento).State = System.Data.Entity.EntityState.Modified;
            Db.SaveChanges();

            return Json(" Atualizado ");
        }

    }
}
