using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DotNetShopping.Models;
using Microsoft.AspNet.Identity;

namespace DotNetShopping.Controllers
{
    public class ManagePageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ManagePage
        public ActionResult Index()
        {
            return View(db.Pages.ToList());
        }

        // GET: ManagePage/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = db.Pages.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        // GET: ManagePage/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManagePage/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "PageId,PageTitle,PageBody,Keywords,Description")] PageEditModel pageedit)
        {
            if (ModelState.IsValid)
            {
                if (User != null)
                {
                    var p = db.Pages.Find(pageedit.PageId);
                    if (p != null)
                    {
                        ViewBag.Error = "PageId is in use!";
                    }
                    else
                    {
                        var userId = User.Identity.GetUserId();
                        var today = DateTime.Now;
                        var page = new Page();
                        page.PageId = pageedit.PageId;
                        page.PageTitle = pageedit.PageTitle;
                        page.PageBody = pageedit.PageBody;
                        page.Keywords = pageedit.Keywords;
                        page.Description = pageedit.Description;
                        page.CreateDate = today;
                        page.CreateUser = userId;
                        page.UpdateDate = today;
                        page.UpdateUser = userId;

                        db.Pages.Add(page);
                        db.SaveChanges();
                    }

                }

                return RedirectToAction("Index");
            }

            return View(pageedit);
        }

        // GET: ManagePage/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = db.Pages.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            var pageedit = new PageEditModel();
            pageedit.PageId = page.PageId;
            pageedit.PageTitle = page.PageTitle;
            pageedit.PageBody = page.PageBody;
            pageedit.Description = page.Description;
            pageedit.Keywords = page.Keywords;
            ViewBag.EditPageId = page.PageId;
            return View(pageedit);
        }

        // POST: ManagePage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PageId,PageTitle,PageBody,Keywords,Description")] PageEditModel pageedit,string EditPageId)
        {
            if (ModelState.IsValid)
            {

                var userId = User.Identity.GetUserId();
                var today = DateTime.Now;
                var page = db.Pages.Find(pageedit.PageId);
                if (page != null)
                {
                    if (EditPageId != pageedit.PageId)
                    {
                        var p = db.Pages.Find(EditPageId);
                        if (p != null)
                        {
                            ViewBag.Error = "PageId is in use!";
                            return View(pageedit);
                        }
                        else
                        {
                            db.Pages.Remove(page);

                            var newpage = new Page();
                            newpage.PageId = EditPageId;
                            newpage.PageTitle = pageedit.PageTitle;
                            newpage.PageBody = pageedit.PageBody;
                            newpage.Keywords = pageedit.Keywords;
                            newpage.Description = pageedit.Description;
                            newpage.CreateDate = today;
                            newpage.CreateUser = userId;
                            newpage.UpdateDate = today;
                            newpage.UpdateUser = userId;

                            db.Pages.Add(newpage);
                            await db.SaveChangesAsync();
                            return RedirectToAction("Index");
                            
                        }
                    }
                    else
                    {
                        page.PageTitle = pageedit.PageTitle;
                        page.PageBody = pageedit.PageBody;
                        page.Keywords = pageedit.Keywords;
                        page.Description = pageedit.Description;
                        page.CreateDate = today;
                        page.UpdateDate = today;
                        page.CreateUser = userId;
                        page.UpdateUser = userId;

                        db.Entry(page).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Index");
            }
            return View(pageedit);
        }

        // GET: ManagePage/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = db.Pages.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        // POST: ManagePage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Page page = db.Pages.Find(id);
            db.Pages.Remove(page);
            db.SaveChanges();
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
