using System;
using System.Collections.Generic;
using System.Linq;
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

            var order = db.Table_OrderDetails.Where(o => o.Table_Orders.CartID == db.Table_Cart.Where(c => c.UserID == userId).FirstOrDefault().CartID);
            return View(order);
        }

        public ActionResult AddOrder(Table_Cart cart)
        {
            int userId = db.Table_User
                .Where(u => u.PhoneNumber == User.Identity.Name)
                .Select(u => u.ID)
                .SingleOrDefault();

            var userOrder = db.Table_Orders.Where(o => o.Table_Cart.UserID == userId).FirstOrDefault();

            if (userOrder == null)
            {
                userOrder = new Table_Orders
                {
                    CartID = cart.CartID,
                    TotalCost = cart.TotalCost,
                    OrderDate = DateTime.Now,
                };
                db.Table_Orders.Add(userOrder);
                db.SaveChanges();
            }


            var cartItems = db.Table_CartItem
                .Where(ci => ci.CartID == cart.CartID)
                .ToList();

            foreach (var cartItem in cartItems)
            {
                var existingOrderDetail = db.Table_OrderDetails
           .Where(od => od.OrderID == userOrder.OrderID && od.ProductID == cartItem.ProductID)
           .FirstOrDefault();

                if (existingOrderDetail != null)
                {
                    existingOrderDetail.Quantity += cartItem.Quantity;
                }
                else
                {
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

            foreach (var cartItem in cartItems)
            {
                cartItem.Table_Products.ProductQuantity -= cartItem.Quantity;
            }

            foreach (var cartItem in cartItems)
            {
                db.Table_CartItem.Remove(cartItem);
            }
            var userCart = db.Table_Cart.Where(c => c.CartID == cart.CartID).FirstOrDefault();
            userCart.TotalCost = 0;

            db.SaveChanges();
            return RedirectToAction("Index");
            

        }

        public ActionResult ConfirmOrder(int id)
        {

            return View();
        }
    }
}