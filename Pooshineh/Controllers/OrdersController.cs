using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Pooshineh.Models;

namespace Pooshineh.Controllers
{
    public class OrdersController : Controller
    {
        ClothingStoreEntities1 db = new ClothingStoreEntities1();
        public ActionResult Index()
        {
            int userId = db.Table_User
                .Where(u => u.PhoneNumber == User.Identity.Name)
                .Select(u => u.ID)
                .SingleOrDefault();

            var order = db.Table_Cart
        .Where(c => c.UserID == userId)
        .FirstOrDefault();
            var orders = db.Table_CartItem.Where(ci => ci.Table_Cart.UserID == userId);

            return View(orders);
        }
        
        public ActionResult ConfirmOrder(Table_Cart cart)
        {
            if (cart != null && cart.UserID != 0)
            {
                int userId = db.Table_User
                    .Where(u => u.PhoneNumber == User.Identity.Name)
                    .Select(u => u.ID)
                    .SingleOrDefault();

                var userCart = db.Table_Cart
                    .Where(c => c.UserID == userId && c.CartID == cart.CartID)
                    .FirstOrDefault();

                if (userCart != null)
                {
                    var userOrder = new Table_Orders
                    {
                        CartID = userCart.CartID,
                        TotalCost = userCart.TotalCost,
                        OrderDate = DateTime.Now,
                        OrderName = Guid.NewGuid().ToString().Replace("-", ""),
                        OrderStatus = "pending"
                    };

                    db.Table_Orders.Add(userOrder);

                    var cartItems = db.Table_CartItem
                        .Where(ci => ci.CartID == userCart.CartID)
                        .ToList();

                    foreach (var cartItem in cartItems)
                    {
                        var product = db.Table_Products.Find(cartItem.ProductID);
                        if (product != null)
                        {
                            product.ProductQuantity -= cartItem.Quantity;

                            var orderDetail = new Table_OrderDetails
                            {
                                OrderID = userOrder.OrderID,
                                ProductID = cartItem.ProductID,
                                Quantity = cartItem.Quantity,
                                Price = cartItem.Table_Products.ProductPrice
                            };

                            db.Table_OrderDetails.Add(orderDetail);
                        }
                    }

                    db.Table_CartItem.RemoveRange(cartItems);
                    userCart.TotalCost = 0;

                    db.SaveChanges();

                    return RedirectToAction("ViewConfirmedOrder");
                }
                
            }
            return RedirectToAction("Index");
        }

        public ActionResult ViewConfirmedOrder()
        {
            var order = db.Table_Orders
                .Where(o => o.Table_Cart.Table_User.PhoneNumber == User.Identity.Name)
                .OrderByDescending(o => o.OrderID)
                .FirstOrDefault();
            return View(order);
        }

        public ActionResult ViewActiveOrders()
        {
            var orders = db.Table_Orders.Where(o => o.Table_Cart.Table_User.PhoneNumber == User.Identity.Name);
            return View(orders);
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Table_Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
    }
}