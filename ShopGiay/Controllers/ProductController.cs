using ShopGiay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ShopGiay.Controllers
{
    public class ProductController : Controller
    {
        private Data db = new Data();

        // GET: Product
        public ActionResult Index()
        {
            var account = db.Products.ToList();
            return View(account);
        }
        public ActionResult ProductDetails(string productId)
        {
            // Lấy thông tin sản phẩm từ cơ sở dữ liệu hoặc từ nguồn dữ liệu khác
            var product = db.Products.FirstOrDefault(p => p.ProductId == productId);

            // Kiểm tra xem sản phẩm có tồn tại không
            if (product == null)
            {
                // Nếu không tồn tại, có thể chuyển hướng hoặc xử lý theo ý muốn
                return HttpNotFound(); // Trả về mã lỗi 404 Not Found
            }

            // Chuyển dữ liệu sản phẩm đến view để hiển thị
            return View(product);
        }
    }
}