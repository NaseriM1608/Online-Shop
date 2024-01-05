using Pooshineh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pooshineh.Controllers
{
    public class HomeController : Controller
    {
        ClothingStoreEntities1 db = new ClothingStoreEntities1();
        public ActionResult Index()
        {
            var productsList = db.Table_Products.ToList();
            return View(productsList);
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
    }
}