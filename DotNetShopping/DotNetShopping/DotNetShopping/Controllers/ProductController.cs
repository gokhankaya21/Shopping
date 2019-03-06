using DotNetShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductCreateModel model)
        {
            return View();
        }


        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(ProductEditModel model)
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(Int64 ProductId)
        {
            var product = db.Products.Find(ProductId);
            product.Archived = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Variants()
        {
            return View();
        }

        public ActionResult VariantCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VariantCreate(VariantCreateModel model)
        {
            return View();
        }


        public ActionResult VariantEdit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VariantEdit(VariantEditModel model)
        {
            return View();
        }

        public ActionResult VariantDelete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VariantDelete(Int64 VariantId)
        {
            var variant = db.Variants.Find(VariantId);
            variant.Archived = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}