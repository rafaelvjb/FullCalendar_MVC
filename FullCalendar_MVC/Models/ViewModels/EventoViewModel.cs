using System;
using System.ComponentModel.DataAnnotations;

namespace FullCalendar_MVC.Models.ViewModels
{
    public class EventoViewModel
    {
        public int ID { get; set; }

        public Guid ConvenioId { get; set; }

        [Required]
        public String Titulo { get; set; }

        public String DataEvento { get; set; }

        public String HoraEvento { get; set; }

        public bool Consulta { get; set; }

        public bool Retorno { get; set; }

        public String DuracaoEvento { get; set; }

        public String Observacoes { get; set; }
        [Required(ErrorMessage = "Este campo não pode ser vazio")]
        public String ProfissionalId { get; set; }
    }
}