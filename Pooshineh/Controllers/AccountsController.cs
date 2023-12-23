using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Pooshineh.Models;

namespace Pooshineh.Controllers
{
    public class AccountsController : Controller
    {
        ClothingStoreEntities db = new ClothingStoreEntities();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel credentials)
        {
            if(ModelState.IsValid)
            {
                bool userExists = db.Table_Customer.Any(t => t.PhoneNumber == credentials.PhoneNumber && t.Password == credentials.Password);
                var customer = db.Table_Customer.FirstOrDefault(t => t.PhoneNumber == credentials.PhoneNumber);
                if(userExists)
                {
                    FormsAuthentication.SetAuthCookie(customer.PhoneNumber, false);
                    return RedirectToAction("Contact", "Home");
                }
                ModelState.AddModelError("", "اطلاعات وارد شده صحیح نمی‌باشد.");
                return View();
            }
            return View();
        }
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp([Bind(Include = "PhoneNumber, Password")]Table_Customer newCustomer)
        {
            if(ModelState.IsValid)
            {
                db.Table_Customer.Add(newCustomer);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}