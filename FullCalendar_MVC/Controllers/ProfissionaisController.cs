using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using FullCalendar_MVC.Models;

namespace FullCalendar_MVC.Controllers
{
    public class ProfissionaisController : Contoller
    {
        //private AgendaOnlineFc db = new AgendaOnlineFc();

        // GET: Profissionais
        public async Task<ActionResult> Index()
        {
            return View(await Db.Profissionais.ToListAsync());
        }

        // GET: Profissionais/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profissional profissional = await Db.Profissionais.FindAsync(id);
            if (profissional == null)
            {
                return HttpNotFound();
            }
            return View(profissional);
        }

        // GET: Profissionais/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profissionais/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProfissionalId,Nome,Ativo")] Profissional profissional)
        {
            if (!ModelState.IsValid) return View(profissional);
            profissional.ProfissionalId = Guid.NewGuid();
            Db.Profissionais.Add(profissional);
            await Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Profissionais/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profissional profissional = await Db.Profissionais.FindAsync(id);
            if (profissional == null)
            {
                return HttpNotFound();
            }
            return View(profissional);
        }

        // POST: Profissionais/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProfissionalId,Nome,Ativo")] Profissional profissional)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(profissional).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(profissional);
        }

        // GET: Profissionais/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profissional profissional = await Db.Profissionais.FindAsync(id);
            if (profissional == null)
            {
                return HttpNotFound();
            }
            return View(profissional);
        }

        // POST: Profissionais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var profissional = await Db.Profissionais.FindAsync(id);
            if (profissional != null) Db.Profissionais.Remove(profissional);
            await Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
