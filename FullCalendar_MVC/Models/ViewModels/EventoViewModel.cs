using System;

namespace FullCalendar_MVC.Models.ViewModels
{
    public class EventoViewModel
    {
        public Int16 ID { get; set; }

        public String Titulo { get; set; }

        public String DataEvento { get; set; }

        public String HoraEvento { get; set; }

        public String DuracaoEvento { get; set; }

        public String Observacoes { get; set; }

        public String ProfissionalId { get; set; }
    }
}