using ShopGiay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShopGiay.Controllers
{
    public class ReportsController : Controller
    {
        private Data db = new Data();
        // GET: Reports
        public ActionResult Index()
        {
            double? totalRevenue = db.Orders.Sum(o => o.TotalPrice);
            int totalItemsSold = db.Orders.Count();
            var orders = db.Orders.ToList(); // Lấy danh sách các đối tượng Order từ cơ sở dữ liệu

            ViewBag.TotalRevenue = totalRevenue ?? 0;
            ViewBag.TotalItemsSold = totalItemsSold != null ? totalItemsSold : 0;

            return View(orders); // Truyền danh sách Order vào view
        }

        public ActionResult DeleteOrder() {
            return View();
        }
        [HttpPost]
        public ActionResult DeleteOrder(string orderId)
        {
            if (orderId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Tìm hóa đơn cần xóa trong cơ sở dữ liệu
            var order = db.Orders.Find(orderId);

            if (order == null)
            {
                return HttpNotFound();
            }

            // Xóa hóa đơn khỏi cơ sở dữ liệu
            db.Orders.Remove(order);
            db.SaveChanges();

            

            // Chuyển hướng về trang danh sách hóa đơn (hoặc trang tương tự)
            return RedirectToAction("Index");
        }


    }
}