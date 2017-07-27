using System;
using System.Linq;
using FullCalendar_MVC.Models.Contexto;
using FullCalendar_MVC.Models.ViewModels;

namespace FullCalendar_MVC.Models.Validacoes
{
    public class ValidaHorario 
    {
        public readonly AgendaOnlineFc Db = new AgendaOnlineFc();

        public bool PossuiAgendamentoEditar(EventoViewModel evento, DateTime fim)
        {
            var possuiAgendamento = Db.Eventos.FirstOrDefault(
                e => e.ProfissionalId == Guid.Parse(evento.ProfissionalId) &&
                     e.end <= fim);

            return true;
        }

        public bool PossuiAgendamentoSalvar(EventoViewModel evento, DateTime fim, DateTime start)
        {
            var diminui = new TimeSpan(0,1,0);
            fim = fim.Subtract(diminui);
            var profissionalId =Guid.Parse(evento.ProfissionalId);
            var possuiAgendamento = Db.Eventos.FirstOrDefault(e => e.start == start  && e.ProfissionalId == profissionalId);
            if (possuiAgendamento != null ) { return true;}
            return false;
        }

        public DateTime AdicionaMinuto(DateTime horario)
        {
            var addMinutes = horario.AddMinutes(1);
            return addMinutes;
        }
    }
}