using System;
using FullCalendar_MVC.Models.ViewModels;

namespace FullCalendar_MVC.Models.Validacoes
{
    public class ValidaData
    {
        private bool DataMariorQueDataAtual(DateTime data)
        {
            if (data <= DateTime.Now) return false;
            return true;
        }

        public DateTime FormataDataInicial(EventoViewModel eventos)
        {
            var data = ObtemHoraMinuto(eventos.HoraEvento);//DateTime.Parse(eventos.DataEvento);
            return DateTime.Now;
        }

        public double[] ObtemHoraMinuto(string horario)
        {
            var horaMinuto = horario.Split(':');
            var hora = Convert.ToDouble(horaMinuto[0]);
            var minuto = Convert.ToDouble(horaMinuto[1]);

             double[] dados = {hora, minuto};
            return dados;
        }


    }
}