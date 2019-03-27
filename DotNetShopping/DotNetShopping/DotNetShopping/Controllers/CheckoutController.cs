﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetShopping.Models;
using Microsoft.AspNet.Identity;

namespace DotNetShopping.Controllers
{
    public class CheckoutController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Checkout
        public ActionResult Cart()
        {
            CartListModel co = new CartListModel();
            var cart = co.GetCart(User.Identity.GetUserId());
            return View(cart);
        }
        [HttpPost]
        public ActionResult Cart(List<CartListModel> cartForm)
        {
            var userId = User.Identity.GetUserId();
            var carts = db.Carts.Where(x => x.UserId == userId).ToList();
            foreach (Cart cart in carts)
            {
                int formValue = cartForm.Where(x => x.VariantId == cart.VariantId).FirstOrDefault().Quantity;
                if(cart.Quantity!=formValue)
                {
                    cart.Quantity = formValue;
                }
            }
            db.SaveChanges();
            CartListModel co = new CartListModel();
            var model = co.GetCart(User.Identity.GetUserId());
            return View(model);
        }

        public ActionResult DeleteCart(Int64 VariantId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var UserId = User.Identity.GetUserId();
                var cart = db.Carts.Where(x => x.UserId == UserId && x.VariantId == VariantId).FirstOrDefault();
                if (cart != null)
                {
                    db.Carts.Remove(cart);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Cart");
        }

        public ActionResult DeleteAllCart()
        {
           if (User.Identity.IsAuthenticated)
           {
               var UserId = User.Identity.GetUserId();
               var carts = db.Carts.Where(x => x.UserId == UserId).ToList();
               if (carts != null)
               {
                   db.Carts.RemoveRange(carts);
                   db.SaveChanges();
               }
           }
           return RedirectToAction("Cart");
        }

        public ActionResult Checkout()
        {
            return View();
        }
    }
}