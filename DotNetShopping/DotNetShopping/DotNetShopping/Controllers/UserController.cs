using DotNetShopping.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Controllers
{
    public class UserController : Controller
    {
        private IdentityDbContext db = new IdentityDbContext();
        // GET: User
        public ActionResult Index()
        {
            var users = db.Users.Include("Roles").ToList();
            return View(users);
        }
    }
}