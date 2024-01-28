using Pooshineh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Pooshineh.Controllers
{
    [Authorize]
    public class DiscountsController : Controller
    {
        ClothingStoreEntities1 db = new ClothingStoreEntities1();
        public ActionResult Index()
        {
            var discounts = db.Table_Discounts.ToList();
            return View(discounts);
        }
        public ActionResult AddDiscountCode() 
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDiscountCode(Table_Discounts newDiscount)
        {
            if(ModelState.IsValid)
            {
                db.Table_Discounts.Add(newDiscount);
                db.SaveChanges();
                TempData["DiscountAddSuccess"] = "کد تخفیف با موفقیت اضاف شد.";
                return RedirectToAction("Index");
            }
            return View(newDiscount);
        }
        public ActionResult ApplyDiscountCode(DiscountViewModel Discount)
        {
            var user = db.Table_User
                    .Where(u => u.PhoneNumber == User.Identity.Name).FirstOrDefault();

            bool codeExists = db.Table_Discounts.Any(d => d.DiscountCode == Discount.DiscountCode);
            var userOrder = db.Table_Cart.Where(c => c.UserID == user.ID).FirstOrDefault();
            var DiscountRepititionCheck = user.Table_Cart.Where(c => c.UserID == user.ID).FirstOrDefault(o => o.DiscountCode == Discount.DiscountCode);

            if (codeExists && DiscountRepititionCheck == null)
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
            else if(codeExists && userOrder.DiscountCode != null && userOrder.DiscountCode != DiscountRepititionCheck.DiscountCode)
            {
                TempData["DiscountUsed"] = "فقط یک بار می‌توانید کد تخفیف وارد کنید.";
            }
            else if(DiscountRepititionCheck != null)
            {
                TempData["DiscountRepeat"] = "شما این کد تخفیف را استفاده نموده‌اید.";
            }

            return RedirectToAction("Index","Orders");
        }

        public ActionResult Delete(int id)
        {
            var discount = db.Table_Discounts.Where(d => d.DiscountID == id).FirstOrDefault();
            db.Table_Discounts.Remove(discount);
            TempData["DeletionSuccess"] = "کد تخفیف با موفقیت حذف شد.";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
    
}