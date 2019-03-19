using DotNetShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Controllers
{
    public class ProductDetailController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ProductDetail
        public ActionResult Product(Int64 id,string name)
        {
            var products = db.Products.Include("Brands")
                .Join(db.Variants, p => p.ProductId, v => v.ProductId, (p, v) => new { Product = p, Variant = v })
                .Join(db.Categories, pv => pv.Product.CategoryId, c => c.CategoryId, (pv, c) => new { pv, Category = c })
                .Where(x => x.pv.Product.Archived == false && x.pv.Variant.Archived == false)
                .Select(x => new ProductDetailModel
                {
                    ProductId = x.pv.Product.ProductId,
                    VariantId=x.pv.Variant.VariantId,
                    ProductName=x.pv.Product.Name,
                    VariantName=x.pv.Variant.Name,
                    CategoryId=x.pv.Product.CategoryId,
                    CategoryName=x.Category.Name,
                    BrandId=x.pv.Product.BrandId,
                    BrandName=x.pv.Product.Brand.Name,
                    Description=x.pv.Product.Description,
                    IsVisible=x.pv.Product.IsVisible,
                    OnSale=x.pv.Product.OnSale,
                    Stock=x.pv.Variant.Stock,
                    UnitPrice=x.pv.Variant.UnitPrice,
                    Images=
                });
            return View();
        }
    }
}