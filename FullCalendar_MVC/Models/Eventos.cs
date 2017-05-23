using System;
using System.ComponentModel.DataAnnotations;

namespace FullCalendar_MVC.Models
{
    public class Eventos
    {
        [Key]
        public int ID { get; set; }
        public string title { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public int StatusEnum { get; set; }
    }
}