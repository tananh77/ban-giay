using ShopGiay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShopGiay.Controllers
{
    public class CartController : Controller
    {
        private Data db = new Data();

        // GET: Cart
        public ActionResult Index()
        {
            // Kiểm tra xem Session có tồn tại hay không
            if (Session["UserId"] == null)
            {
                // Nếu không tồn tại, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login", "Account");
            }

            // Lấy UserId từ Session hoặc Identity của người dùng
            var userId = Session["UserId"] as string;

            // Kết nối bảng Cart và Product để lấy thông tin sản phẩm trong giỏ hàng
            var cartItems = (from cart in db.Carts
                             where cart.UserId == userId && cart.Condition == "InCart"
                             join product in db.Products on cart.ProductId equals product.ProductId
                             select new CartItemViewModel
                             {
                                 ProductId = cart.ProductId,
                                 Name = product.Name,
                                 Price = product.Price,
                                 Quantity = cart.Quantity,
                                 Image = product.Image,
                                 Available = product.Quantity,
                             }).ToList();

            return View(cartItems);
        }



        [HttpPost]
        public ActionResult AddToCart(string productId, int quantity)
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (Session["UserId"] == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login", "Account");
            }

            // Lấy UserId từ Session hoặc Identity của người dùng
            var userId = Session["UserId"] as string;

            // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng của người dùng chưa
            var existingCartItem = db.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

            if (existingCartItem != null)
            {
                // Nếu đã tồn tại, cập nhật số lượng
                existingCartItem.Quantity += quantity;
            }
            else
            {
                // Nếu chưa tồn tại, thêm mới vào giỏ hàng
                var newCartItem = new Cart
                {
                    CartId = Guid.NewGuid().ToString(), // Tạo một CartId mới
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    Condition = "InCart" // Điều kiện mặc định khi thêm vào giỏ hàng
                };

                db.Carts.Add(newCartItem);
            }

            db.SaveChanges();

            // Chuyển hướng về trang giỏ hàng hoặc trang sản phẩm tùy bạn
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult BuyNow(string productId)
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (Session["UserId"] == null)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login", "Account");
            }

            // Lấy UserId từ Session hoặc Identity của người dùng
            var userId = Session["UserId"] as string;

            // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng của người dùng chưa
            var existingCartItem = db.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

            if (existingCartItem != null)
            {
                // Nếu đã tồn tại, cập nhật số lượng
                existingCartItem.Quantity += 1;
            }
            else
            {
                // Nếu chưa tồn tại, thêm mới vào giỏ hàng với số lượng là 1
                var newCartItem = new Cart
                {
                    CartId = Guid.NewGuid().ToString(), // Tạo một CartId mới
                    UserId = userId,
                    ProductId = productId,
                    Quantity = 1, // Số lượng mặc định là 1
                    Condition = "InCart" // Điều kiện mặc định khi thêm vào giỏ hàng
                };

                db.Carts.Add(newCartItem);
            }

            db.SaveChanges();

            // Chuyển hướng về trang giỏ hàng hoặc trang sản phẩm tùy bạn
            return RedirectToAction("Index", "Cart");
        }
        [HttpPost]
        public ActionResult UpdateQuantity(string productId, int change)
        {
            // Lấy UserId từ Session hoặc Identity của người dùng
            var userId = Session["UserId"] as string;

            // Lấy sản phẩm trong giỏ hàng
            var cartItem = db.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId && c.Condition == "InCart");

            if (cartItem != null)
            {
                // Cập nhật số lượng
                cartItem.Quantity += change;

                // Kiểm tra nếu số lượng bằng 0 thì xóa sản phẩm
                if (cartItem.Quantity == 0)
                {
                    db.Carts.Remove(cartItem);
                }

                db.SaveChanges();
            }

            // Trả về trạng thái OK
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        [HttpPost]
        public ActionResult DeleteProduct(string productId)
        {
            // Lấy UserId từ Session hoặc Identity của người dùng
            var userId = Session["UserId"] as string;

            // Lấy sản phẩm trong giỏ hàng
            var cartItem = db.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId && c.Condition == "InCart");

            if (cartItem != null)
            {
                // Xóa sản phẩm
                db.Carts.Remove(cartItem);
                db.SaveChanges();
            }

            // Trả về trạng thái OK
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

    }
}
