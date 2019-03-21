﻿using DotNetShopping.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Controllers
{
    public class ApiController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public ActionResult AddToCart(Int64 VariantId, int Qty)
        {
            if (User.Identity.IsAuthenticated)
            {
                var UserId = User.Identity.GetUserId();
                var cart = db.Carts.Where(x => x.UserId == UserId && x.VariantId == VariantId).FirstOrDefault();
                if (cart != null)
                {
                    cart.Quantity += Qty;
                    db.SaveChanges();
                }
                else
                {
                    var newCart = new Cart();
                    newCart.UserId = UserId;
                    newCart.VariantId = VariantId;
                    newCart.Quantity = Qty;
                    db.Carts.Add(newCart);
                    db.SaveChanges();
                }
            }
            return Json(new { Success = true });
        }
        public ActionResult GetShoppingCart()
        {
            var UserId = User.Identity.GetUserId();
            return Json(new { Success = true });
        }
        private IEnumerable<CartListModel> GetCart(string UserId)
        {
            var model = db.Carts
                .Join(db.Variants, c => c.VariantId, v => v.VariantId, (c, v) => new { Cart = c, Variant = v })
                .Join(db.Products, cv => cv.Variant.ProductId, p => p.ProductId, (cv, p) => new { cv, Product = p })
                .Where(x => x.cv.Cart.UserId == UserId)
                .Select(x => new CartListModel
                {
                    UserId = x.cv.Cart.UserId,
                    VariantId = x.cv.Cart.VariantId,
                    ProductId = x.cv.Variant.ProductId,
                    Quantity = x.cv.Cart.Quantity,
                    VariantName = x.cv.Variant.Name,
                    ProductName = x.cv.Variant.Product.Name,
                    UnitPrice = x.cv.Variant.UnitPrice,
                    TotalPrice = x.cv.Variant.UnitPrice * x.cv.Cart.Quantity
                }).ToList();
            return model;
        }
    }
}