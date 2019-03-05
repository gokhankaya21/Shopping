using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DotNetShopping.Models;

namespace DotNetShopping.Controllers
{
    public class StatesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: States
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            var states = db.States.Include(s => s.Country);
            return View(await states.ToListAsync());
        }

        // GET: States/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            State state = await db.States.FindAsync(id);
            if (state == null)
            {
                return HttpNotFound();
            }
            return View(state);
        }

        // GET: States/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "Code");
            return View();
        }

        // POST: States/Create
        [Authorize(Roles = "Admin")]
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "StateId,Name,Code,CountryId")] State state)
        {
            if (ModelState.IsValid)
            {
                db.States.Add(state);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "Code", state.CountryId);
            return View(state);
        }

        // GET: States/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            State state = await db.States.FindAsync(id);
            if (state == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "Code", state.CountryId);
            return View(state);
        }

        // POST: States/Edit/5
        [Authorize(Roles = "Admin")]
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "StateId,Name,Code,CountryId")] State state)
        {
            if (ModelState.IsValid)
            {
                db.Entry(state).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "Code", state.CountryId);
            return View(state);
        }

        // GET: States/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            State state = await db.States.FindAsync(id);
            if (state == null)
            {
                return HttpNotFound();
            }
            return View(state);
        }

        // POST: States/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(short id)
        {
            State state = await db.States.FindAsync(id);
            db.States.Remove(state);
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
