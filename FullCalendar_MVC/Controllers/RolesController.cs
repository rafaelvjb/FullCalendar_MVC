using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using FullCalendar_MVC.Models.Contexto;
using FullCalendar_MVC.Models.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FullCalendar_MVC.Controllers
{
    public class RolesController : Contoller
    {
        // private AgendaOnlineFc db = new AgendaOnlineFc();
        public ApplicationRoleManager _roleManager = new ApplicationRoleManager(new RoleStore<Grupo, Guid, UsuarioGrupo>(new AgendaOnlineFc()));
        // GET: Roles
        public async Task<ActionResult> Index()
        {
            return View(await Db.Roles.ToListAsync());
        }

        // GET: Roles/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupo grupo =  Db.Roles.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }
            return View(grupo);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Grupo grupo)
        {
            if (ModelState.IsValid)
            {
                grupo.Id = Guid.NewGuid();
                Db.Roles.Add(grupo);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(grupo);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupo grupo =  Db.Roles.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }
            return View(grupo);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Grupo grupo)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(grupo).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(grupo);
        }

        // GET: Roles/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupo grupo =  Db.Roles.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }
            return View(grupo);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Grupo grupo =  Db.Roles.Find(id);
            Db.Roles.Remove(grupo);
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
