using Pooshineh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Pooshineh.Controllers
{
    public class CartController : Controller
    {
        ClothingStoreEntities1 db = new ClothingStoreEntities1();
        public ActionResult Index()
        {

            int userId = db.Table_User
                .Where(u => u.PhoneNumber == User.Identity.Name)
                .Select(u => u.ID)
                .SingleOrDefault();

            // Get the user's cart and cart items
            var userCart = db.Table_CartItem
    .Where(cartItem => cartItem.Table_Cart.UserID == userId)
    .Select(cartItem => cartItem.Table_Products)
    .ToList();

            if (userCart == null)
            {
                return View();
            }

            return View(userCart);
        }

        [Authorize]
        public ActionResult AddToCart(int productId, int quantity)
        {
            if (ModelState.IsValid)
            {
                int userId = db.Table_User
            .Where(u => u.PhoneNumber == User.Identity.Name)
            .Select(u => u.ID)
            .SingleOrDefault();

                var userCart = db.Table_Cart.SingleOrDefault(c => c.UserID == userId);

                if (userCart == null)
                {
                    userCart = new Table_Cart
                    {
                        UserID = userId,
                    };

                    db.Table_Cart.Add(userCart);
                    db.SaveChanges();
                }

                var existingCartItem = db.Table_CartItem
                    .SingleOrDefault(ci => ci.CartID == userCart.CartID && ci.ProductID == productId);

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += quantity;
                    db.SaveChanges();
                }
                else
                {
                    var product = db.Table_Products.Find(productId);

                    if (product != null)
                    {
                        var newCartItem = new Table_CartItem
                        {
                            CartID = userCart.CartID,
                            ProductID = productId,
                            Quantity = quantity,
                            Price = product.ProductPrice
                        };

                        db.Table_CartItem.Add(newCartItem);
                        db.SaveChanges();

                    }
                }
               
                userCart.TotalCost = db.Table_CartItem
                    .Where(ci => ci.CartID == userCart.CartID)
                    .Sum(ci => ci.Quantity * ci.Price);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            int userId = db.Table_User
            .Where(u => u.PhoneNumber == User.Identity.Name)
            .Select(u => u.ID)
            .SingleOrDefault();

            var product = db.Table_CartItem.Where(ci => ci.ProductID == id && ci.Table_Cart.UserID == userId).SingleOrDefault();
            db.Table_CartItem.Remove(product);
            db.SaveChanges();

            var userCart = db.Table_Cart.SingleOrDefault(c => c.UserID == userId);

            if(db.Table_CartItem.Where(ci => ci.CartID == userCart.CartID).SingleOrDefault() != null)
                userCart.TotalCost = db.Table_CartItem
                    .Where(ci => ci.CartID == userCart.CartID)
                    .Sum(ci => ci.Quantity * ci.Price);

            else userCart.TotalCost = 0;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}