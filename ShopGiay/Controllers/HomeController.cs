using ShopGiay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopGiay.Controllers
{
    public class HomeController : Controller
    {
        private Data db = new Data();

        public ActionResult Index()
        {
            List<Product> products = db.Products.ToList();
            return View(products);
        }
    }
}