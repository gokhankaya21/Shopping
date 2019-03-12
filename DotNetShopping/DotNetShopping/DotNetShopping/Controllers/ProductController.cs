using DotNetShopping.Helpers;
using DotNetShopping.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
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
                    UnitPrice = x.pv.Variant.UnitPrice,
                    PhotoName = db.ProductImages.Where(i => i.VariantId == x.pv.Variant.VariantId).OrderBy(i => i.Sequence).FirstOrDefault().FileName
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
            ViewBag.SupplierId = new SelectList(db.Suppliers.OrderBy(x => x.Name), "SupplierId", "Name", model.SupplierId);
            ViewBag.BrandId = new SelectList(db.Brands.OrderBy(x => x.Name), "BrandId", "Name", model.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x => x.Name), "CategoryId", "Name", model.CategoryId);
            return View();
        }


        public ActionResult Edit(Int64 id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                var model = new ProductEditModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    BrandId = product.BrandId,
                    SupplierId = product.SupplierId,
                    CategoryId = product.CategoryId,
                    Description = product.Description,
                    IsVisible = product.IsVisible,
                    OnSale = product.OnSale,
                    Unit = product.Unit
                };
                ViewBag.SupplierId = new SelectList(db.Suppliers.OrderBy(x => x.Name), "SupplierId", "Name", model.SupplierId);
                ViewBag.BrandId = new SelectList(db.Brands.OrderBy(x => x.Name), "BrandId", "Name", model.BrandId);
                ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x => x.Name), "CategoryId", "Name", model.CategoryId);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", new { Error = "Product Not Found" });
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductEditModel model)
        {
            var product = db.Products.Find(model.ProductId);
            if (product != null)
            {
                product.Name = model.ProductName;
                product.BrandId = model.BrandId;
                product.CategoryId = model.CategoryId;
                product.Description = model.Description;
                product.IsVisible = model.IsVisible;
                product.OnSale = model.OnSale;
                product.SupplierId = model.SupplierId;
                product.Unit = model.Unit;
                product.UpdateDate = DateTime.Today;
                product.UpdateUser = User.Identity.GetUserId();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.SupplierId = new SelectList(db.Suppliers.OrderBy(x => x.Name), "SupplierId", "Name", model.SupplierId);
                ViewBag.BrandId = new SelectList(db.Brands.OrderBy(x => x.Name), "BrandId", "Name", model.BrandId);
                ViewBag.CategoryId = new SelectList(db.Categories.OrderBy(x => x.Name), "CategoryId", "Name", model.CategoryId);
                return View(model);
            }

        }

        public ActionResult Delete(Int64 id)
        {
            var product = db.Products.Find(id);
            product.Archived = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Variants(Int64 id)
        {
            var model = db.Variants.Where(x => x.ProductId == id && !x.Archived)
                .Select(x => new VariantListModel

                {
                    VariantId = x.VariantId,
                    VariantName = x.Name,
                    Cost = x.Cost,
                    IsVisible = x.IsVisible,
                    ProductId = x.ProductId,
                    Stock = x.Stock,
                    UnitPrice = x.UnitPrice,
                    PhotoName = db.ProductImages.Where(i => i.VariantId == x.VariantId).OrderBy(i => i.Sequence).FirstOrDefault().FileName
                });
            return View(model);
        }

        public ActionResult VariantCreate(Int64 id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                var model = new VariantCreateModel();
                model.ProductId = id;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", new { Error = "Product Not Found" });
            }
        }

        [HttpPost]
        public ActionResult VariantCreate(VariantCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var today = DateTime.Today;
                var variant = new Variant();
                variant.ProductId = model.ProductId;
                variant.Name = model.VariantName;
                variant.IsVisible = model.IsVisible;
                variant.Stock = model.Stock;
                variant.UnitPrice = model.UnitPrice;
                variant.CreateDate = today;
                variant.UpdateDate = today;
                variant.CreateUser = User.Identity.GetUserId();
                variant.UpdateUser = User.Identity.GetUserId();
                db.Variants.Add(variant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }


        public ActionResult VariantEdit(Int64 id)
        {
            var variant = db.Variants.Find(id);
            if (variant != null)
            {
                var model = new VariantEditModel();
                model.ProductId = variant.ProductId;
                model.VariantId = variant.VariantId;
                model.VariantName = variant.Name;
                model.UnitPrice = variant.UnitPrice;
                model.Stock = variant.Stock;
                model.IsVisible = variant.IsVisible;
                model.Cost = variant.Cost;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", new { Error = "Variant Not Found" });
            }
        }

        [HttpPost]
        public ActionResult VariantEdit(VariantEditModel model)
        {
            var variant = db.Variants.Find(model.VariantId);
            if (variant != null)
            {
                variant.ProductId = model.ProductId;
                variant.VariantId = model.VariantId;
                variant.Name = model.VariantName;
                variant.UnitPrice = model.UnitPrice;
                variant.Stock = model.Stock;
                variant.IsVisible = model.IsVisible;
                variant.Cost = model.Cost;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", new { Error = "Variant Not Found" });
            }
        }

        public ActionResult VariantDelete(Int64 id)
        {
            var variant = db.Variants.Find(id);
            variant.Archived = true;
            db.SaveChanges();
            return RedirectToAction("Variants", new { id = variant.ProductId });
        }
        public ActionResult PhotoAdd(Int64 id, Int64 ProductId)
        {
            ViewBag.Error = "";
            var model = new PhotoAddModel();
            model.VariantId = id;
            model.ProductId = ProductId;
            return View(model);
        }

        [HttpPost]
        public ActionResult PhotoAdd(PhotoAddModel model)
        {
            ViewBag.Error = "";
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.UploadedFile != null)
                    {
                        var image = System.Drawing.Image.FromStream(model.UploadedFile.InputStream);
                        if (image.Width >= 1000 && image.Height >= 1000)
                        {
                            var productImage = new ProductImage();
                            string fileName = productImage.InsertProductImage(model.VariantId);
                            ImageHelper.SaveImage(fileName, image);
                        }
                        else
                        {
                            throw new Exception("Photo needs to be minimum 1000px X 1000px size");
                        }
                    }
                }
                return RedirectToAction("Variants", new { id = model.ProductId });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }
    }
}