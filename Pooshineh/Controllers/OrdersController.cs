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
            return View();
        }

        public ActionResult AddOrder(Table_Cart cart)
        {
            int userId = db.Table_User
             .Where(u => u.PhoneNumber == User.Identity.Name)
             .Select(u => u.ID)
             .SingleOrDefault();

            var userOrder = new Table_Orders
            {
                CartID = cart.CartID,
                TotalCost = cart.TotalCost,
                OrderDate = DateTime.Now,
                OrderName = Guid.NewGuid().ToString().Replace("-", "")
            };

            db.Table_Orders.Add(userOrder);

            var cartItems = db.Table_CartItem
             .Where(ci => ci.CartID == cart.CartID)
             .ToList();

            foreach (var cartItem in cartItems)
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
            foreach (var cartItem in cartItems)
            {
                cartItem.Table_Products.ProductQuantity -= cartItem.Quantity;
            }


            db.SaveChanges();
            return View();
        }
    }
}