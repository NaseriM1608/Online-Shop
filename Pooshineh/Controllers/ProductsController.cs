﻿using Pooshineh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Data.Entity;
using System.Web.Helpers;

namespace Pooshineh.Controllers
{

    public class ProductsController : Controller
    {
        ClothingStoreEntities1 db = new ClothingStoreEntities1();
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
        public ActionResult CreateProduct(Table_Products newProduct, HttpPostedFileBase newImage, string[] Colors, int[] Quantities)
        {
            string newImageName = "";

            if (ModelState.IsValid)
            {
                if (newImage != null)
                {
                    if (newImage.ContentType != "image/jpeg" && newImage.ContentType != "image/png")
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

                int productId = newProduct.ProductID;
                var Sizes = db.Table_ProductSize.ToList();
                int k = 0;

                if (Colors != null && Sizes != null && Quantities != null)
                {
                    for (int i = 0; i < Colors.Length; i++)
                    {
                        string color = Colors[i];
                        for (int j = 0; j < Sizes.Count; j++)
                        {
                            int quantity = Quantities[k];
                            Table_ProductDetails productDetails = new Table_ProductDetails { ProductID = productId, ProductSizeID = Sizes[j].ProductSizeID, Color = color, Quantity = quantity };
                            db.Table_ProductDetails.Add(productDetails);
                            k++;
                        }
                    }
                }
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
        public ActionResult Edit(Table_Products product, HttpPostedFileBase newImage)
        {
            string newImageName;
            if (ModelState.IsValid)
            {
                if (newImage != null)
                {
                    if (newImage.ContentType != "image/jpeg" && newImage.ContentType != "image/png")
                    {
                        ModelState.AddModelError("ProductImagePath", "فرمت عکس باید به صورت jpg, jpeg یا png باشد.");
                        return View(product);
                    }
                    newImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(newImage.FileName);
                    newImage.SaveAs(Server.MapPath("~/Images/Products/") + newImageName);
                    product.ProductImagePath = newImageName;
                }
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                TempData["ProductEditSuccess"] = "تغییرات با موفقیت انجام شد.";
                return RedirectToAction("Edit");
            }
            TempData["ProductEditFailed"] = "لطفا فرم را به درستی پر کنید.";
            return View(product);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var product = db.Table_Products.Find(id);
            db.Table_Products.Remove(product);
            var productsInCart = db.Table_CartItem.Where(ci => ci.ProductID == id);

            foreach (var productInCart in productsInCart)
            {
                var cart = db.Table_Cart.Where(c => c.CartID == productInCart.CartID).FirstOrDefault();
                cart.TotalCost -= productInCart.Quantity * productInCart.Price;
                db.Table_CartItem.Remove(productInCart);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int? id)
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
        public ActionResult GetQuantity(int productId, string size, string color)
        {
            int availableQuantity = db.Table_ProductDetails.Where(pd => pd.ProductID == productId && pd.Table_ProductSize.Size == size && pd.Color == color).FirstOrDefault().Quantity;
            return Json(new { availableQuantity, JsonRequestBehavior.AllowGet });
        }
        public ActionResult GetColors(int productId, string size)
        {
            var colors = db.Table_ProductDetails.Where(pd => pd.ProductID == productId && pd.Table_ProductSize.Size == size).Select(pd => pd.Color).ToList();
            return Json(new { colors, JsonRequestBehavior.AllowGet });
        }




    }
}