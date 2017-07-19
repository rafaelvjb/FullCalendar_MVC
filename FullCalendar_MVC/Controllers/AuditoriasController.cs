using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FullCalendar_MVC.Controllers
{
    public class AuditoriasController : FullCalendar_MVC.Controllers.Contoller
    {
        // GET: Auditorias
        public ActionResult Index()
        {
            return View();
        }


        public async Task<ActionResult> AgendamentoAuditoria()
        {
            var lista = await Db.Eventos.ToListAsync();

            return View(lista);
        }


    }
}