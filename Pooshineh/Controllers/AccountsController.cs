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
                return RedirectToAction("Index");
            }
            return View(user);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer= db.Table_User.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var customer = db.Table_User.Find(id);
            db.Table_User.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}