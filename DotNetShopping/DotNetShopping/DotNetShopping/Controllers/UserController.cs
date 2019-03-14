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
            var users = db.Users.Select(x => new
            {
                UserId = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                RoleNames = x.Roles.Join(db.Roles, u => u.RoleId, r => r.Id, (u, r) => new { u, r }).Select(ur => ur.r.Name).ToList()
            }).ToList().Select(x => new UserListModel
            {
                Email = x.Email,
                UserId = x.UserId,
                Roles = string.Join(",", x.RoleNames)
            }).ToList();
            return View(users);
        }
        public ActionResult Edit(string UserId)
        {
            var user = db.Users.Find(UserId);
            return View(user);
        }
        public ActionResult Roles()
        {
            var roles = db.Roles.OrderBy(x => x.Name).ToList();
            return View(roles);
        }
    }
}