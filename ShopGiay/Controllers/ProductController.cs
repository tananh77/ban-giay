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
        [HttpGet]
        public ActionResult Search(string searchText)
        {
            // Kiểm tra xem searchText có tồn tại hay không
            if (string.IsNullOrEmpty(searchText))
            {
                // Nếu không có, chuyển hướng về trang chủ hoặc hiển thị thông báo lỗi
                return RedirectToAction("Index");
            }

            // Chạy truy vấn tìm kiếm trong cơ sở dữ liệu
            var searchResults = db.Products.Where(p => p.Name.Contains(searchText)).ToList();

            // Truyền kết quả tìm kiếm đến view
            return View(searchResults);
        }
    }
}