﻿using DotNetShopping.Models;
using Microsoft.AspNet.Identity;
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
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var products = db.Products.Include("Brands").Include("Suppliers")
                .Join(db.Variants, p => p.ProductId, v => v.ProductId, (p, v) => new { Product = p, Variant = v })
                .Join(db.Categories, pv => pv.Product.CategoryId, c => c.CategoryId, (pv, c) => new { pv, Category = c })
                .Where(x => x.pv.Product.Archived == false && x.pv.Variant.Archived == false)
                .Select(x => new ProductListModel
                {
                    ProductId = x.pv.Product.ProductId,
                    VariantId = x.pv.Variant.VariantId,
                    ProductName = x.pv.Product.Name,
                    VariantName = x.pv.Variant.Name,
                    BrandName = x.pv.Product.Brand.Name,
                    SupplierName = x.pv.Product.Supplier.Name,
                    CategoryName = x.Category.Name,
                    Cost = x.pv.Variant.Cost,
                    IsVisible = x.pv.Product.IsVisible == true ? x.pv.Variant.IsVisible : false,
                    OnSale = x.pv.Product.OnSale,
                    Stock = x.pv.Variant.Stock,
                    UnitPrice = x.pv.Variant.UnitPrice
                });
            return View(products.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.SupplierId = new SelectList(db.Suppliers.OrderBy(x => x.Name), "SupplierId", "Name");
            ViewBag.BrandId = new SelectList(db.Brands.OrderBy(x => x.Name), "BrandId", "Name");
            ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x => x.Name), "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductCreateModel model)
        {
            try
            {
                var today = DateTime.Today;
                var p = new Product();
                p.Name = model.ProductName;
                p.BrandId = model.BrandId;
                p.Archived = false;
                p.CategoryId = model.CategoryId;
                p.CreateDate = today;
                p.CreateUser = User.Identity.GetUserId();
                p.UpdateDate = today;
                p.UpdateUser = User.Identity.GetUserId();
                p.Unit = model.Unit;
                p.SupplierId = model.SupplierId;
                p.OnSale = model.OnSale;
                p.IsVisible = model.IsVisible;
                p.Description = model.Description;
                db.Products.Add(p);
                db.SaveChanges();

                var v = new Variant();
                v.ProductId = p.ProductId;
                v.Archived = false;
                v.Cost = model.Cost;
                v.CreateDate = today;
                v.CreateUser = User.Identity.GetUserId();
                v.UpdateDate = today;
                v.UpdateUser = User.Identity.GetUserId();
                v.IsVisible = model.IsVisible;
                v.Name = model.VariantName;
                v.Stock = model.Stock;
                v.UnitPrice = model.UnitPrice;
                db.Variants.Add(v);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            var products = db.Products.OrderBy(x => x.Name).ToList();
            products.Insert(0, new Product { Name = "No Product" });
            ViewBag.SupplierId = new SelectList(db.Suppliers.OrderBy(x => x.Name), "SupplierId", "Name", model.SupplierId);
            ViewBag.BrandId = new SelectList(db.Brands.OrderBy(x => x.Name), "BrandId", "Name", model.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x => x.Name), "CategoryId", "Name", model.CategoryId);
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