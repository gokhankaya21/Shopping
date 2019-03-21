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
        // GET: Api
        [HttpPost]
        public ActionResult AddToCart(Int64 VariantId, int Qty)
        {
            var UserId = User.Identity.GetUserId();
            return Json(new { Success = true });
        }
    }
}