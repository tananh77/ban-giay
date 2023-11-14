using ShopGiay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ShopGiay.Controllers
{
    public class AccountController : Controller
    {
        private Data db = new Data();

        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                string user = username.Trim();
                string pass = password.Trim();
                if (user == "" || pass == "")
                {
                    ViewBag.Notification = "Please complete all information";
                }
                else
                {
                    var data = db.Accounts.FirstOrDefault(s => s.Username == user && s.Password == pass);
                    if (data != null)
                    {
                        Session["UserId"] = data.UserId;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Notification = "Error";
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string username, string password, string email, int? phone)
        {
            // Kiểm tra dữ liệu đầu vào và thực hiện đăng ký
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(email))
            {
                // Kiểm tra xem username đã tồn tại trong CSDL chưa
                if (IsUsernameAvailable(username))
                {
                    // Thực hiện xử lý đăng ký ở đây (ví dụ: thêm thông tin vào cơ sở dữ liệu)
                    // Chú ý: Trong ứng dụng thực tế, bạn cần kiểm tra và xử lý dữ liệu một cách an toàn hơn

                    var newUser = new Account
                    {
                        UserId = "IDUs" + username,
                        Username = username.Trim(),
                        Password = password.Trim(),
                        Role = "user"
                    };
                    var newIFUser = new InforAccount
                    {
                        InforAccountId = "IDIF" + username,
                        UserId = "IDUs" + username,
                        Email = email.Trim(),
                        Phone = phone
                    };


                    
                    db.InforAccounts.Add(newIFUser);
                    db.Accounts.Add(newUser);
                    db.SaveChanges();

                    // Chuyển hướng đến trang đăng nhập hoặc trang khác tùy ý
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    // Hiển thị thông báo lỗi nếu username đã tồn tại
                    ViewBag.Notification = "Username is not available. Please choose another one.";
                    return View();
                }
            }
            else
            {
                // Hiển thị thông báo lỗi nếu có
                ViewBag.Notification = "Please complete all information";
                return View();
            }
        }
        // Hàm kiểm tra xem username đã tồn tại trong CSDL chưa
        private bool IsUsernameAvailable(string username)
        {
            return !db.Accounts.Any(u => u.Username == username);
        }

        [HttpGet]
        public ActionResult InforAccount()
        {
            // Lấy thông tin tài khoản từ Session hoặc cơ sở dữ liệu (tùy thuộc vào cách bạn quản lý đăng nhập)
            string userId = Session["UserId"]?.ToString();
            var account = db.Accounts.FirstOrDefault(a => a.UserId == userId);

            if (account != null)
            {
                var inforAccount = db.InforAccounts.FirstOrDefault(i => i.UserId == userId);

                var accountModel = new AccountModel
                {
                    UserId = account.UserId,
                    Username = account.Username,
                    Password = account.Password,
                    Role = account.Role,

                    Firstname = inforAccount?.Firstname,
                    Lastname = inforAccount?.Lastname,
                    Email = inforAccount?.Email,
                    Phone = inforAccount?.Phone,
                    Address = inforAccount?.Address,
                    Sex = inforAccount?.Sex,
                    Image = inforAccount?.Image
                };

                return View(accountModel);
            }

            // Xử lý trường hợp không tìm thấy tài khoản
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public ActionResult UpdateProfile(int? phone, string email, bool sex, string address, string firstname, string lastname)
        {
            try
            {
                // Lấy UserId từ Session hoặc từ Identity của người dùng, tùy thuộc vào cách bạn quản lý người dùng
                var userId = Session["UserId"] as string;

                // Lấy thông tin người dùng từ cơ sở dữ liệu
                var user = db.InforAccounts.FirstOrDefault(u => u.UserId == userId);

                // Cập nhật thông tin
                if (user != null)
                {
                    // Cập nhật các trường thông tin
                    user.Phone = phone;
                    user.Email = email;
                    user.Address = address;
                    user.Firstname = firstname;
                    user.Lastname = lastname;
                    user.Sex = sex;

                        // Lưu thay đổi vào cơ sở dữ liệu
                        db.SaveChanges();

                        // Trả về kết quả thành công
                        TempData["SuccessMessage"] = "Profile updated successfully!";
                }
                else
                {
                    TempData["SuccessMessage"] = "Error!";
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về kết quả lỗi
                TempData["ErrorMessage"] = "An error occurred while updating profile. Please try again later.";
            }

            // Chuyển hướng về trang người dùng sau khi cập nhật xong
            return RedirectToAction("InforAccount");
        }

        public ActionResult Logout()
        {
            Session["UserId"] = null;
            return RedirectToAction("Login", "Account");
        }


    }
}