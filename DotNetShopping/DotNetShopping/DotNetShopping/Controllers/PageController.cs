﻿using DotNetShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Controllers
{
    public class PageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Page
        [OutputCache(Duration = 36000,VaryByParam ="PageId")]
        public ActionResult Index(string PageId)
        {
            var page = db.Pages.Find(PageId);
            if (page != null)
            {
                ViewBag.Title = page.PageTitle;
                ViewBag.Keywords = page.Keywords;
                ViewBag.Description = page.Description;
                ViewBag.Body = page.PageBody;
                return View(page);
            }
            else
            {
                return RedirectToAction("PageNotFound","Home");
            }


        }
        
    }
}