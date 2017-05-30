using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
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
            // ReSharper disable once RedundantBoolCompare
            ViewBag.Profissionais = Db.Profissionais
                .Where(p => p.Ativo == true)
                .ToList();
            return View();
        }

        //lista todos os eventos
        public JsonResult Eventos(string start, string end, string usuarioId)
        {
            var listaConvertida = new List<Eventos>();
            var dtInicial = Convert.ToDateTime(start).Date;
            var dtfinal = Convert.ToDateTime(end).Date;
            IQueryable<Eventos> queryable = Db.Eventos;
            if (!String.IsNullOrEmpty(usuarioId))
            {
                var id = Guid.Parse(usuarioId);
                queryable = queryable.Where(d => d.ProfissionalId == id);
            }

            queryable = queryable.Where(e => e.Profissional.Ativo == true);
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
            if (id == null) return Json(new { message = "Problema ao deletar Evento!!!" }, JsonRequestBehavior.AllowGet);
            var lol = Convert.ToInt32(id);
            var evento = Db.Eventos.FirstOrDefault(e => e.ID == lol);
            Db.Eventos.Remove(evento);
            Db.SaveChanges();
            return Json(new { message = "sucesso" }, JsonRequestBehavior.AllowGet);
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
                return Json(new { message = "Não é possivel gravar um evento com a data anterior que a atual" });
            }
            Db.Eventos.Add(evento);
            Db.SaveChanges();
            return Json(new { message = "Evento salvo com sucesso!" });
            //return RedirectToAction("Index", "Home");
        }

        // Atualiza a duração do evento
        public JsonResult AtualizaDuracao(int id, string NewEventStart, string NewEventEnd)
        {
            Auditoria(id, NewEventStart, NewEventEnd);
            var evento = Db.Eventos.FirstOrDefault(e => e.ID == id);
            evento.ID = id;
            evento.start = Convert.ToDateTime(NewEventStart);
            evento.end = Convert.ToDateTime(NewEventEnd);

            var tm = new TimeSpan(0, 1, 0);
            var convertido = evento.end.Subtract(tm);
            if (evento == null) return Json(new { message = "Falha ao atualizar eventos" });
            var verificaExistencia = Db.Eventos
                .FirstOrDefault(d => d.start >= evento.start && d.end <= convertido);

            if (verificaExistencia != null && evento.ID != verificaExistencia.ID)
                return Json(new { message = "Falha ao atualizar eventos" });

            if (evento.end <= DateTime.Now)
                return Json(new { message = "Não é possivel gravar um evento com a data anterior que a atual" });


            Db.Entry(evento).State = System.Data.Entity.EntityState.Modified;
            Db.SaveChanges();
            return Json(new { message = "Sucesso" });
        }

        public void Auditoria(int id, string NewEventStart, string NewEventEnd)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            var evento = Db.Eventos.FirstOrDefault(e => e.ID == id);
            if (evento == null) return;

            var eventoAuditoria = new EventoAuditoria();
            eventoAuditoria.EventoAuditoriaId = Guid.NewGuid();
            eventoAuditoria.Identificacao = id;
            eventoAuditoria.Titulo = evento.title;

            eventoAuditoria.DataIniAntiga = evento.start;
            eventoAuditoria.DataFimAntiga = evento.end;

            eventoAuditoria.DataIniNova = Convert.ToDateTime(NewEventStart);
            eventoAuditoria.DataFimNova = Convert.ToDateTime(NewEventEnd);

            eventoAuditoria.UsuarioModificacao = User.Identity.Name;
            Db.EventoAuditoria.Add(eventoAuditoria);
            Db.SaveChanges();

        }
    }
}