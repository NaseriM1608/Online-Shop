using Pooshineh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pooshineh.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        ClothingStoreEntities1 db = new ClothingStoreEntities1();
        public ActionResult Index()
        {
            var categories = db.Table_Categories.ToList();
            return View(categories);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryName")]Table_Categories newCategory)
        {
            if(ModelState.IsValid)
            {
                db.Table_Categories.Add(newCategory);
                db.SaveChanges();
                TempData["CategoryAdditionSuccess"] = "درج دسته‌بندی موفقیت آمیز بود.";
            }
            return View();
        }

    }
}