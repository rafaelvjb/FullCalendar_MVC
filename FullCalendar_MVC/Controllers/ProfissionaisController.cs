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
        [Authorize]
        public async Task<ActionResult> Index()
        {
            return View(await Db.Profissionais.ToListAsync());
        }

        // GET: Profissionais/Details/5
        [Authorize]
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
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profissionais/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public  ActionResult Create([Bind(Include = "ProfissionalId,Nome,Ativo")] Profissional profissional)
        {
            if (!ModelState.IsValid) return View(profissional);
            profissional.ProfissionalId = Guid.NewGuid();
            Db.Profissionais.Add(profissional);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Profissionais/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Profissional profissional =  Db.Profissionais.Find(id);

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
        [Authorize]
        public ActionResult Edit([Bind(Include = "ProfissionalId,Nome,Ativo")] Profissional profissional)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(profissional).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(profissional);
        }

        // GET: Profissionais/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profissional profissional =  Db.Profissionais.Find(id);
            if (profissional == null)
            {
                return HttpNotFound();
            }
            return View(profissional);
        }

        // POST: Profissionais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public  ActionResult DeleteConfirmed(Guid id)
        {
            var profissional =  Db.Profissionais.Find(id);
            if (profissional != null) Db.Profissionais.Remove(profissional);
            Db.SaveChangesAsync();
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
