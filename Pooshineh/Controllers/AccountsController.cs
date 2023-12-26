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
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "PhoneNumber, Password")]Table_User newCustomer)
        {
            newCustomer.RegisterDate = DateTime.Now;
            newCustomer.IsActive = true;
            if(ModelState.IsValid)
            {
                db.Table_User.Add(newCustomer);
                db.SaveChanges();
                return RedirectToAction("About", "Home");
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
        public ActionResult Edit(Table_Products customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
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