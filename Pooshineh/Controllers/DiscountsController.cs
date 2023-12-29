using Pooshineh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Pooshineh.Controllers
{
    public class DiscountsController : Controller
    {
        ClothingStoreEntities1 db = new ClothingStoreEntities1();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ApplyDiscountCode(DiscountViewModel Discount)
        {
            int userId = db.Table_User
                    .Where(u => u.PhoneNumber == User.Identity.Name)
                    .Select(u => u.ID)
                    .SingleOrDefault();
            bool codeExists = db.Table_Discounts.Any(d => d.DiscountCode == Discount.DiscountCode);
            var userOrder = db.Table_Cart.Where(c => c.UserID == userId).FirstOrDefault();
            if (codeExists && userOrder.DiscountCode == null)
            {
                var discount = db.Table_Discounts.Where(d => d.DiscountCode == Discount.DiscountCode).FirstOrDefault();

                userOrder.TotalCost = userOrder.TotalCost - (userOrder.TotalCost * discount.TotalDiscount / 100);
                userOrder.DiscountCode = Discount.DiscountCode;
                db.SaveChanges();

                TempData["DiscountSuccess"] = "کد تخفیف اعمال شد";
            }
            else if(!codeExists)
            {
                TempData["DiscountError"] = "کد تخفیف نامعتبر است.";
            }
            else if(codeExists && userOrder.DiscountCode != null)
            {
                TempData["DiscountUsed"] = "فقط یک بار می‌توانید کد تخفیف وارد کنید.";
            }

            return RedirectToAction("Index","Orders");
        }

    }
    
}