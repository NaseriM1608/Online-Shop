using Pooshineh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Data.Entity;

namespace Pooshineh.Controllers
{
    public class ProductsController : Controller
    {
        ClothingStoreEntities db = new ClothingStoreEntities();
        public ActionResult Index()
        {
            var products = db.Table_Products;
            return View(products);
        }
        [HttpGet]
        public ActionResult CreateProduct()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProduct(Table_Products newProduct, HttpPostedFileBase newImage)
        {
            string newImageName = "";

            if(ModelState.IsValid)
            {
                if(newImage != null)
                {
                    if(newImage.ContentType != "image/jpeg" &&  newImage.ContentType != "image/png")
                    {
                        ModelState.AddModelError("ProductImagePath", "فرمت عکس باید به صورت jpg, jpeg یا png باشد.");
                        return View(newProduct);
                    }
                    newImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(newImage.FileName);
                    newImage.SaveAs(Server.MapPath("~/Images/Products/") + newImageName);
                }
                newProduct.ProductImagePath = newImageName;
                db.Table_Products.Add(newProduct);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(newProduct);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Table_Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Table_Products product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product = db.Table_Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = db.Table_Products.Find(id);
            db.Table_Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



    }
}