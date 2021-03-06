﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using FullCalendar_MVC.Models;
using FullCalendar_MVC.Models.ViewModels;
using FullCalendar_MVC.Models.Validacoes;

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

            ViewBag.Convenios = Db.Convenio
               .ToList();
            return View();
        }

        //lista todos os eventos
        [SuppressMessage("ReSharper", "JoinDeclarationAndInitializer")]
        [SuppressMessage("ReSharper", "NotAccessedVariable")]
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

            foreach (var item in lista)
            {
                string valor;
                valor = item.Consulta ? "Consulta" : "Retorno";
                var evento = new Eventos
                {
                    ID = item.ID,
                    title = $"{item.title}  - {valor}  - {item.Convenio.Nome}",
                    start = Convert.ToDateTime(item.start),
                    end = Convert.ToDateTime(item.end)
                };

                listaConvertida.Add(evento);
            }
            return Json(listaConvertida, JsonRequestBehavior.AllowGet);
        }

        //Atualiza Evento
        public async Task<JsonResult> AtualizaEvento(EventoViewModel eventos)
        {
            var data = DateTime.Parse(eventos.DataEvento);
            var hora = eventos.HoraEvento.Split(':');
            data = data.AddHours(double.Parse(hora[0]));
            var start = data.AddMinutes(double.Parse(hora[1]));

            var datafim = DateTime.Parse(eventos.DataEvento);
            hora = eventos.DuracaoEvento.Split(':');
            datafim = datafim.AddHours(double.Parse(hora[0]));
            var end = datafim.AddMinutes(double.Parse(hora[1]));

            var validaHorario = new ValidaHorario();

            var possuiAgendamento = validaHorario.PossuiAgendamentoEditar(eventos, start, end);
            if (await possuiAgendamento)
            {
                return Json(new { message = "Este profissional já possui um agendamento neste horario!" }, JsonRequestBehavior.AllowGet);
            }

            var ev = new Eventos()
            {
                ID = eventos.ID,
                title = eventos.Titulo,
                start = start,
                end = end,
                ConvenioId = eventos.ConvenioId,
                ProfissionalId = Guid.Parse(eventos.ProfissionalId),
                Observacoes = eventos.Observacoes
            };

            Db.Entry(ev).State = EntityState.Modified;
            Db.SaveChanges();
            return Json(new { message = "Alterado com Sucesso" }, JsonRequestBehavior.AllowGet);
        }

        //Deleta Evento
        public JsonResult DeletaEvento(int? id)
        {
            if (id == null) return Json(new { message = "Problema ao deletar Evento!!!" }, JsonRequestBehavior.AllowGet);
            var idEvento = Convert.ToInt32(id);
            var evento = Db.Eventos.FirstOrDefault(e => e.ID == idEvento);
            if (evento != null) Db.Eventos.Remove(evento);
            Db.SaveChanges();
            return Json(new { message = "sucesso" }, JsonRequestBehavior.AllowGet);
        }

        //Salva o Evento
        public JsonResult SalvaEvento(EventoViewModel eventos)
        {
            var evento = new Eventos();
            evento.title = eventos.Titulo;
            evento.DataCriacao = DateTime.Now;
            evento.UsuarioCriacao = HttpContext.User.Identity.Name;

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

            var validaHorario = new ValidaHorario();
            var possuiAgendamento = validaHorario.PossuiAgendamentoSalvar(eventos, evento.end, evento.start);
            if (possuiAgendamento)
            {
                return Json(new { message = "Este profissional já possui um agendamento neste horario!" }, JsonRequestBehavior.AllowGet);
            }

            evento.Consulta = eventos.Consulta;
            evento.Retorno = eventos.Retorno;

            evento.ProfissionalId = Guid.Parse(eventos.ProfissionalId);
            evento.ConvenioId = eventos.ConvenioId;
            evento.Observacoes = eventos.Observacoes;

            if (evento.start <= DateTime.Now)
            {
                return Json(new { message = "Não é possivel gravar um evento com a data anterior que a atual" }, JsonRequestBehavior.AllowGet);
            }
            Db.Eventos.Add(evento);
            Db.SaveChanges();
            return Json(new { message = "Evento salvo com sucesso!" }, JsonRequestBehavior.AllowGet);
        }

        // Atualiza a duração do evento
        public async Task<JsonResult> AtualizaDuracao(int id, string NewEventStart, string NewEventEnd)
        {
            var evento = Db.Eventos.FirstOrDefault(e => e.ID == id);
            var ev = new EventoViewModel();
            ev.ID = id;
            ev.ProfissionalId = Convert.ToString(evento.ProfissionalId);

            evento.ID = id;
            evento.start = Convert.ToDateTime(NewEventStart);
            evento.end = Convert.ToDateTime(NewEventEnd);

            var validaHorario = new ValidaHorario();

            var possuiAgendamento = validaHorario.PossuiAgendamentoEditar(ev,evento.start, evento.end);
            if (await possuiAgendamento)
            {
                return Json(new { message = "Possui Agendamento" }, JsonRequestBehavior.AllowGet);
            }

            if (evento.end <= DateTime.Now)
                return Json(new { message = "Não é possivel gravar um evento com a data anterior que a atual" }, JsonRequestBehavior.AllowGet);

            Db.Entry(evento).State = EntityState.Modified;

            Db.SaveChanges();
            return Json(new { message = "Sucesso" }, JsonRequestBehavior.AllowGet);
        }

        //Obterm por Id
        public JsonResult ObtemPorId(int id)
        {
            var eventos = Db.Eventos.FirstOrDefault(e => e.ID == id);
            Debug.Assert(eventos != null, "eventos != null");
            var evento = new Eventos
            {
                ID = eventos.ID,
                title = eventos.title,
                start = Convert.ToDateTime(eventos.start),
                end = Convert.ToDateTime(eventos.end),
                ProfissionalId = eventos.ProfissionalId,
                Observacoes = eventos.Observacoes,
                ConvenioId = eventos.ConvenioId,
                Consulta = eventos.Consulta,
                Retorno = eventos.Retorno
            };

            ViewBag.HoraInicio = evento.start.TimeOfDay;
            ViewBag.HoraFim = evento.end.TimeOfDay;
            return Json(evento, JsonRequestBehavior.AllowGet);
        }
    }
}