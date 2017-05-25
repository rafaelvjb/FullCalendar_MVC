using System;
using System.Linq;
using FullCalendar_MVC.Models.Contexto;
using FullCalendar_MVC.Models.ViewModels;


// << dont forget to add this for converting dates to localtime

namespace FullCalendar_MVC.Models
{
    public class DiaryEvent
    {

        public int ID;
        public string Title;
        public int SomeImportantKeyID;
        public string StartDateString;
        public string EndDateString;
        public string StatusString;
        public string StatusColor;
        public string ClassName;


        //public static List<DiaryEvent> LoadAllAppointmentsInDateRange(double start, double end)
        //{
        //    var fromDate = ConvertFromUnixTimestamp(start);
        //    var toDate = ConvertFromUnixTimestamp(end);
        //    using (var ent = new AgendaOnlineFc())
        //    {
        //        var rslt = ent.Eventos.Where(s => s.start >= fromDate && EntityFunctions.AddMinutes(s.start, s.end) <= toDate);

        //        List<DiaryEvent> result = new List<DiaryEvent>();
        //        //if (rslt != null)
        //        //{
        //        //    foreach (var item in rslt)
        //        //    {
        //        //        DiaryEvent rec = new DiaryEvent();
        //        //        rec.ID = item.ID;
        //        //        rec.SomeImportantKeyID = item.SomeImportantKey;
        //        //        rec.StartDateString = item.DateTimeScheduled.ToString("s"); // "s" is a preset format that outputs as: "2009-02-27T12:12:22"
        //        //        rec.EndDateString = item.DateTimeScheduled.Dte(item.AppointmentLength).ToString("s"); // field AppointmentLength is in minutes
        //        //        rec.Title = item.Title + " - " + item.AppointmentLength.ToString() + " mins";
        //        //        rec.StatusString = Enums.GetName<AppointmentStatus>((AppointmentStatus)item.StatusENUM);
        //        //        rec.StatusColor = Enums.GetEnumDescription<AppointmentStatus>(rec.StatusString);
        //        //        string ColorCode = rec.StatusColor.Substring(0, rec.StatusColor.IndexOf(":"));
        //        //        rec.ClassName = rec.StatusColor.Substring(rec.StatusColor.IndexOf(":") + 1, rec.StatusColor.Length - ColorCode.Length - 1);
        //        //        rec.StatusColor = ColorCode;
        //        //        result.Add(rec);
        //        //    }
        //        //}
        //        return result;
        //    }

        //}


        //public static List<DiaryEvent> LoadAppointmentSummaryInDateRange(double start, double end)
        //{

        //    var fromDate = ConvertFromUnixTimestamp(start);
        //    var toDate = ConvertFromUnixTimestamp(end);
        //    using (var ent = new AgendaOnlineFc())
        //    {
        //        //var rslt = ent.Eventos.Where(s => s.start >= fromDate &&
        //        //                                           EntityFunctions.AddMinutes(s.start,
        //        //                                               s.end) <= toDate);
        //                                                //.GroupBy(s => EntityFunctions.TruncateTime(s.DateTimeScheduled))
        //                                                //.Select(x => new { DateTimeScheduled = x.Key, Count = x.Count() });

        //        List<DiaryEvent> result = new List<DiaryEvent>();
        //        int i = 0;
        //        foreach (var item in rslt)
        //        {
        //            DiaryEvent rec = new DiaryEvent();
        //            rec.ID = i; //we dont link this back to anything as its a group summary but the fullcalendar needs unique IDs for each event item (unless its a repeating event)
        //            rec.SomeImportantKeyID = -1;
        //            string StringDate = string.Format("{0:yyyy-MM-dd}", item.start);
        //            rec.StartDateString = StringDate + "T00:00:00"; //ISO 8601 format
        //            rec.EndDateString = StringDate + "T23:59:59";
        //           // rec.Title = "Booked: " + item.Count.ToString();
        //            result.Add(rec);
        //            i++;
        //        }

        //        return result;
        //    }

        //}

        public static void AtualizarEventoDiario(int id, string NewEventStart, string NewEventEnd)
        {
            var ent = new AgendaOnlineFc();
            var rec = ent.Eventos.FirstOrDefault(s => s.ID == id);
            if (rec == null) return;
            rec.start = DateTime.Parse(NewEventStart);
            if (!String.IsNullOrEmpty(NewEventEnd))
            {
                rec.end = Convert.ToDateTime(NewEventEnd);
            }
            ent.Entry(rec).State = System.Data.Entity.EntityState.Modified;
            ent.SaveChanges();
        }


        public static bool CriaNovoEvento(EventoViewModel ev)
        {
            try
            {
                var ent = new AgendaOnlineFc();
                var rec = new Eventos();
                rec.title = ev.Titulo;
                var data = DateTime.Parse(ev.DataEvento);
                var hora = ev.HoraEvento.Split(':');
                data = data.AddHours(Convert.ToDouble(hora[0]));
                data = data.AddMinutes(Convert.ToDouble(hora[1]));
                rec.start = data;

                if (!String.IsNullOrEmpty(ev.DuracaoEvento))
                {
                    var duracao = int.Parse(ev.DuracaoEvento);
                    rec.end = rec.start.AddMinutes(duracao);
                }
                else
                {
                    rec.end = rec.start.AddMinutes(30);
                }
                rec.ProfissionalId = Guid.Parse(ev.ProfissionalId);
                ent.Eventos.Add(rec);
                ent.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        //public static bool CriaNovoEvento(string titulo, string novaDataEvento, string novaHoraEvento, string novoDuracaoEvento,string id)
        //{
        //    try
        //    {
        //        var ent = new AgendaOnlineFc();
        //        var rec = new Eventos();
        //        rec.title = titulo;
        //        var data = DateTime.Parse(novaDataEvento);
        //        var hora = novaHoraEvento.Split(':');
        //        data =  data.AddHours(Convert.ToDouble(hora[0]));
        //        data = data.AddMinutes(Convert.ToDouble(hora[1]));
        //        rec.start = data;
                
        //        if (!String.IsNullOrEmpty(novoDuracaoEvento))
        //        {
        //            var duracao = int.Parse(novoDuracaoEvento);
        //            rec.end = rec.start.AddMinutes(duracao);
        //        }
        //        else
        //        {
        //            rec.end = rec.start.AddMinutes(30);
        //        }
        //        rec.ProfissionalId = Guid.Parse(id);
        //        ent.Eventos.Add(rec);
        //        ent.SaveChanges();
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
    }
}