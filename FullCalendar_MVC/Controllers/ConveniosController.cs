using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using FullCalendar_MVC.Models;
using FullCalendar_MVC.Models.Contexto;

namespace FullCalendar_MVC.Controllers
{
    public class ConveniosController : Controller
    {
        private AgendaOnlineFc db = new AgendaOnlineFc();

        // GET: Convenios
        public async Task<ActionResult> Index()
        {
            return View(await db.Convenio.ToListAsync());
        }

        // GET: Convenios/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convenio convenio = await db.Convenio.FindAsync(id);
            if (convenio == null)
            {
                return HttpNotFound();
            }
            return View(convenio);
        }

        // GET: Convenios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Convenios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ConvenioId,Nome")] Convenio convenio)
        {
            if (ModelState.IsValid)
            {
                convenio.ConvenioId = Guid.NewGuid();
                convenio.DataCriacao = DateTime.Now;
                db.Convenio.Add(convenio);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(convenio);
        }

        // GET: Convenios/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convenio convenio = await db.Convenio.FindAsync(id);
            if (convenio == null)
            {
                return HttpNotFound();
            }
            return View(convenio);
        }

        // POST: Convenios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ConvenioId,Nome")] Convenio convenio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(convenio).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(convenio);
        }

        // GET: Convenios/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convenio convenio = await db.Convenio.FindAsync(id);
            if (convenio == null)
            {
                return HttpNotFound();
            }
            return View(convenio);
        }

        // POST: Convenios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Convenio convenio = await db.Convenio.FindAsync(id);
            db.Convenio.Remove(convenio);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
