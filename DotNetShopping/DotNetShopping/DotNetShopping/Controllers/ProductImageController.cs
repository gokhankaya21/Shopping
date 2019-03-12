using DotNetShopping.Helpers;
using DotNetShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Controllers
{
    public class ProductImageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ProductImage
        public ActionResult Index(Int64 id)
        {
            var images = db.ProductImages.Where(x => x.VariantId == id).OrderBy(x => x.Sequence);
            return View(images.ToList());
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
                return RedirectToAction("Index", new { id = model.VariantId });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }
    }
}