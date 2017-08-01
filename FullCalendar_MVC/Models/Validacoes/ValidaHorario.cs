using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FullCalendar_MVC.Models.Contexto;
using FullCalendar_MVC.Models.ViewModels;

namespace FullCalendar_MVC.Models.Validacoes
{
    public class ValidaHorario
    {
        public readonly AgendaOnlineFc Db = new AgendaOnlineFc();


        //public bool PossuiAgendamentoEditar(EventoViewModel evento, DateTime inicio, DateTime fim)
        //{
        //    var profissionalId = Guid.Parse(evento.ProfissionalId);
        //    var possuiAgendamento = Db.Eventos.FirstOrDefault(e => e.ProfissionalId == profissionalId &&
        //                                                           e.ID != evento.ID &&
        //                                                           e.start >= inicio &&
        //                                                           e.end <= fim);
        //    return possuiAgendamento != null;
        //}

        public async Task<bool> PossuiAgendamentoEditar(EventoViewModel evento, DateTime inicio, DateTime fim)
        {
            var profissionalId = Guid.Parse(evento.ProfissionalId);
            if (await ValidaHoraInicial(evento, inicio, profissionalId))
            {
                return true;
            }

            if (await ValisHorarioFinalMesmoId(evento, inicio, fim))
            {
                return false;
            }

            if (!await ValidaHorarioFinal(evento, fim))
            {
                return true;
            }
            return false;
        }

        public bool PossuiAgendamentoSalvar(EventoViewModel evento, DateTime fim, DateTime start)
        {
            //var diminuiMinuto = new TimeSpan(0,1,0);
            //fim = fim.Subtract(diminuiMinuto);
            var profissionalId = Guid.Parse(evento.ProfissionalId);
            var possuiAgendamento = Db.Eventos.FirstOrDefault(e => e.start == start && e.ProfissionalId == profissionalId);
            return possuiAgendamento != null;
        }

        public DateTime AdicionaMinuto(DateTime horario)
        {
            var addMinutes = horario.AddMinutes(1);
            return addMinutes;
        }

        public async Task<bool> ValidaHoraInicial(EventoViewModel evento, DateTime horaInicial, Guid profissionalId)
        {
            var query = await Db.Eventos.FirstOrDefaultAsync(
                e => e.start == horaInicial && e.ProfissionalId == profissionalId && e.ID != evento.ID);

            return query != null;
        }

        public async Task<bool> ValidaHorarioFinal(EventoViewModel evento, DateTime fim)
        {
            var profissionalId = Guid.Parse(evento.ProfissionalId);
            IQueryable<Eventos> queryable = Db.Eventos;
            queryable = queryable.Where(e => e.ID == evento.ID && e.ProfissionalId == profissionalId);

            if (await queryable.AnyAsync())
            {
                //queryable = queryable.Where(e => e.start >= fim && e.end <= fim);
                //if ()
                //{

                //}
                return false;
            }


            queryable = queryable.Where(e => e.start >= fim && e.end <= fim);
            //queryable = queryable.Where(e => e.end <= fim);
            await queryable.FirstOrDefaultAsync();
            return await queryable.AnyAsync();
        }

        public async Task<bool> ValisHorarioFinalMesmoId(EventoViewModel evento, DateTime inicio, DateTime fim)
        {
            var profissionalId = Guid.Parse(evento.ProfissionalId);
            IQueryable<Eventos> queryable = Db.Eventos;
            queryable = queryable.Where(e => e.ID == evento.ID && e.ProfissionalId == profissionalId);

            if (await queryable.AnyAsync())
            {
                return false;
            }
            return !await queryable.AnyAsync();
        }
    }
}