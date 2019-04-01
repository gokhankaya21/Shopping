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
                CartListModel co = new CartListModel();
                var model = co.GetCart(UserId);
                return Json(new { Success = true, Cart = model });
            }
            return Json(new { Success = true });
        }
        [HttpPost]
        public ActionResult RemoveCart(Int64 VariantId)
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
                CartListModel co = new CartListModel();
                var model = co.GetCart(UserId);
                return Json(new { Success = true, Cart = model });
            }
            return Json(new { Success = true });
        }
        public ActionResult GetShoppingCart()
        {
            CartListModel co = new CartListModel();
            var UserId = User.Identity.GetUserId();
            var model = co.GetCart(UserId);
            return Json(new { Success = true, Cart = model }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetStatesFor(Int16 CountryId)
        {
            var states = db.States.Where(x => x.CountryId == CountryId)
                .OrderBy(x => x.Name).ToList();
            return Json(new { Success = true, States = states }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCitiesFor(Int16 CountryId,Int16 StateId)
        {
            var cities = db.Cities.Where(x => x.CountryId == CountryId && x.StateId == StateId)
                .OrderBy(x => x.Name).ToList();
            return Json(new { Success = true, Cities = cities });
        }

        public ActionResult GetShippingMethods(Int16 CountryId)
        {
            var model = db.ShippingMethods
                .Join(db.ShippingCosts,sm=>sm.ShippingMethodId,sc=>sc.ShippingMethodId,(sm,sc)=>new { ShippingMethod = sm, ShippingCost = sc }).Where(x=>x.ShippingCost.CountryId==CountryId)
                .Select(x => new ShippingListModel{
                    ShippingMethodId=x.ShippingMethod.ShippingMethodId,
                    Name=x.ShippingMethod.Name,
                    Domestic=x.ShippingMethod.Domestic,
                    International=x.ShippingMethod.International,
                    CostHalf=x.ShippingCost.CostHalf,
                    CostOne=x.ShippingCost.CostOne,
                    CostOneHalf=x.ShippingCost.CostOneHalf,
                    CostTwo=x.ShippingCost.CostTwo,
                    CostTwoHalf=x.ShippingCost.CostTwoHalf,
                    CountryId=x.ShippingCost.CountryId
                })
                .OrderBy(x => x.Name).ToList();

            return Json(new { Success = true, ShippingMethods = model }, JsonRequestBehavior.AllowGet);
        }
    }
}