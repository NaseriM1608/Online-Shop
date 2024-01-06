using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Pooshineh.Models;

namespace Pooshineh.Controllers
{
    public class AccountsController : Controller
    {
        ClothingStoreEntities1 db = new ClothingStoreEntities1();
        [Authorize]
        public ActionResult Index()
        {
            var customers = db.Table_User.Where(t => t.RoleID == 0);
            return View(customers);
        }
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
                bool userExists = db.Table_User.Any(t => t.PhoneNumber == credentials.PhoneNumber && t.Password == credentials.Password);
                var customer = db.Table_User.FirstOrDefault(t => t.PhoneNumber == credentials.PhoneNumber);
                if(userExists)
                {
                    FormsAuthentication.SetAuthCookie(customer.PhoneNumber, false);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("PhoneNumber", "اطلاعات وارد شده صحیح نمی‌باشد.");
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
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "PhoneNumber, Password")]Table_User newCustomer, string passwordRepeat)
        {
            var existingUser = db.Table_User.Where(u => u.PhoneNumber == newCustomer.PhoneNumber).FirstOrDefault();
            if(existingUser != null)
            {
                ModelState.AddModelError("PhoneNumber","کاربری با این شماره ثبت نام کرده است.");
                return View();
            }
            
            if(ModelState.IsValid)
            {
                if (newCustomer.Password == passwordRepeat)
                {
                    newCustomer.RegisterDate = DateTime.Now;
                    newCustomer.IsActive = true;
                    newCustomer.RoleID = 0;
                    db.Table_User.Add(newCustomer);
                    db.SaveChanges();
                    LoginViewModel loginUser = new LoginViewModel() { PhoneNumber = newCustomer.PhoneNumber, Password = newCustomer.Password };
                    return Login(loginUser);
                }
                else
                {
                    ModelState.AddModelError("Password", "رمز مشابه تکرار رمز نیست.");
                }
            }
            return View();
        }
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        [HttpGet]
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = db.Table_User.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Table_User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                TempData["UserEditSuccess"] = "تغییرات با موفقیت انجام شد.";
                return View(user);
            }
            TempData["UserEditFailed"] = "تغییرات با مشکل مواجه شد.";
            return View(user);
        }
        [HttpGet]
        [Authorize]
        public ActionResult Delete(int? id)
        {
            var customer = db.Table_User.Find(id);
            var usersProductsInCart = db.Table_CartItem.Where(ci => ci.Table_Cart.UserID == id);
          
            if(usersProductsInCart.Any())
            {
                db.Table_CartItem.RemoveRange(usersProductsInCart);
            }
            var usersCart = db.Table_Cart.Where(c => c.UserID == id).FirstOrDefault();
            if(usersCart != null)
            {
                var usersOrders = db.Table_Orders.Where(o => o.CartID == usersCart.CartID);
                foreach (var order in usersOrders)
                {
                    var usersOrderDetails = db.Table_OrderDetails.Where(od => od.OrderID == order.OrderID);
                    if(usersOrderDetails.Any())
                        db.Table_OrderDetails.RemoveRange(usersOrderDetails);
                }
                db.Table_Orders.RemoveRange(usersOrders);
                db.Table_Cart.Remove(usersCart);
            }
            db.Table_User.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Table_User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
    }
}