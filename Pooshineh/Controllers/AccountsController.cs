﻿using System;
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
                    if(!customer.IsActive)
                    {
                        TempData["AccountSuspended"] = "حساب کاربری شما مسدود شده است. لطفا به پشتیبانی پیام دهید.";
                        return View(credentials);
                    }
                    FormsAuthentication.SetAuthCookie(customer.PhoneNumber, false);
                    Table_LoginHistory LoginHistory = new Table_LoginHistory()
                    {
                        UserID = customer.ID,
                        LoginDate = DateTime.Now
                    };
                    db.Table_LoginHistory.Add(LoginHistory);
                    db.SaveChanges();
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
        public ActionResult SignUp([Bind(Include = "PhoneNumber, Password, PasswordRepeat")]SignUpViewModel newCustomer)
        {
            var existingUser = db.Table_User.Where(u => u.PhoneNumber == newCustomer.PhoneNumber).FirstOrDefault();
            if(existingUser != null)
            {
                ModelState.AddModelError("PhoneNumber","کاربری با این شماره ثبت نام کرده است.");
                return View();
            }
            
            if(ModelState.IsValid)
            {
                db.Table_User.Add(new Table_User
                {
                    RegisterDate = DateTime.Now,
                    IsActive = true,
                    RoleID = 0,
                    PhoneNumber = newCustomer.PhoneNumber,
                    Password = newCustomer.Password
                });
                db.SaveChanges();
                LoginViewModel loginUser = new LoginViewModel() { PhoneNumber = newCustomer.PhoneNumber, Password = newCustomer.Password };
                return Login(loginUser);
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
                var originalUser = db.Table_User.Find(user.ID);

                

                if (originalUser.PhoneNumber != user.PhoneNumber)
                {
                    db.Entry(originalUser).CurrentValues.SetValues(user);
                    db.SaveChanges();
                    TempData["Login"] = "لطفا با شماره جدید خود مجددا ورود کنید.";
                    return RedirectToAction("SignOut");
                }

                db.Entry(originalUser).CurrentValues.SetValues(user);
                db.SaveChanges();

                TempData["UserEditSuccess"] = "تغییرات با موفقیت انجام شد.";
                return View(originalUser);
            }

            TempData["UserEditFailed"] = "تغییرات با مشکل مواجه شد.";
            return View(user);
        }
        [HttpGet]
        [Authorize]
        public ActionResult ChangeStatus(int? id)
        {
            var customer = db.Table_User.Find(id);
            if(customer.IsActive)
            {
                customer.IsActive = false;
               
            }
            else
            {
                customer.IsActive = true;
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
            var user = db.Table_User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        public ActionResult ViewLoginHistory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Table_User.Find(id);
            if(user == null) 
            {
                return HttpNotFound();
            }
            var loginHistory = user.Table_LoginHistory.Where(l => l.UserID == id).ToList();
            return View(loginHistory);

        }
    }
}