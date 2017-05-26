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
            var listaConvertida = new List<Eventos>();
            //var inicio = 0;
            //while (inicio == 0)
            //{
            //    return Json(listaConvertida, JsonRequestBehavior.AllowGet);
            //    inicio++;
            //}
            var dtInicial = Convert.ToDateTime(start).Date;
            var dtfinal = Convert.ToDateTime(end).Date;
            IQueryable<Eventos> queryable = Db.Eventos;
            if (!String.IsNullOrEmpty(usuarioId))
            {
                var id = Guid.Parse(usuarioId);
                queryable = queryable.Where(d => d.ProfissionalId == id);
            }

            var lista = queryable.Where(d => d.end < dtfinal && d.start > dtInicial).ToList();

           
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


            var teste = Db.Eventos
                .FirstOrDefault(d => d.start >= start && d.end <= end);


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
        public JsonResult DeletaEvento(int id)
        {

            if (id !=  null)
            {
                var lol= Convert.ToInt32(id);
                var evento = Db.Eventos.FirstOrDefault(e => e.ID == lol);
                Db.Eventos.Remove(evento);
                Db.SaveChanges();
                return Json(new { message = "sucesso" },JsonRequestBehavior.AllowGet);
            }
           
            return Json(new {message = "Problema ao deletar Evento!!!"},JsonRequestBehavior.AllowGet);
        }

        //Salva o Evento
        public JsonResult SalvaEvento(EventoViewModel eventos)
        {

            var evento = new Eventos();
            evento.title = eventos.Titulo;
            var data = DateTime.Parse(eventos.DataEvento);

            var hora = eventos.HoraEvento.Split(':');
            data = data.AddHours(Convert.ToDouble(hora[0]));
            data = data.AddMinutes(Convert.ToDouble(hora[1]));
            evento.start = data;
            if (!String.IsNullOrEmpty(eventos.DuracaoEvento))
            {
                var duracao = int.Parse(eventos.DuracaoEvento);
                evento.end = evento.start.AddMinutes(duracao);
            }
            else
            {
                evento.end = evento.start.AddMinutes(30);
            }

            evento.ProfissionalId = Guid.Parse(eventos.ProfissionalId);
            if (evento.start <= DateTime.Now)
            {
                return Json(new {message = "Não é possivel gravar um evento com a data anterior que a atual"});
            }
            Db.Eventos.Add(evento);
            Db.SaveChanges();
            return Json(new {message = "sucess"});
            //return RedirectToAction("Index", "Home");
        }

        // Atualiza a duração do evento
        public JsonResult AtualizaDuracao(int id, string NewEventStart, string NewEventEnd)
        {
            var evento = Db.Eventos.FirstOrDefault(e => e.ID == id);
            evento.ID = id;
            evento.start = Convert.ToDateTime(NewEventStart);
            evento.end = Convert.ToDateTime(NewEventEnd);

            TimeSpan tm = new TimeSpan(0, 1, 0);
            var convertido = evento.end.Subtract(tm);

            var teste = Db.Eventos
                .FirstOrDefault(d => d.start >= evento.start && d.end <= convertido);

            // ReSharper disable once InvertIf
            if (teste == null)
            {
                Db.Entry(evento).State = System.Data.Entity.EntityState.Modified;
                Db.SaveChanges();
                return Json(new {message = "Sucesso"});
            }
            return Json(new {message = "Falha"});
        }

    }
}
