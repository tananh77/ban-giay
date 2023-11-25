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
        public ActionResult Login(AccountModel model)
        {
                var user = db.Accounts.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    // Đăng nhập thành công, thực hiện các hành động cần thiết (ví dụ: lưu thông tin vào Session)
                    Session["UserId"] = user.UserId;
                    return RedirectToAction("Index", "Home"); // Chuyển hướng đến trang chính sau khi đăng nhập thành công
                }

                ViewBag.Notification = "Invalid username or password.";
            return View(model);
            }


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(AccountModel model)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào và thực hiện đăng ký
                if (ModelState.IsValid)
                {
                    // Kiểm tra xem username đã tồn tại trong CSDL chưa
                    if (IsUsernameAvailable(model.Username))
                    {
                        // Thực hiện xử lý đăng ký ở đây (ví dụ: thêm thông tin vào cơ sở dữ liệu)
                        // Chú ý: Trong ứng dụng thực tế, bạn cần kiểm tra và xử lý dữ liệu một cách an toàn hơn

                        var newUser = new Account
                        {
                            UserId = "IDUs" + model.Username,
                            Username = model.Username.Trim(),
                            Password = model.Password.Trim(),
                            Role = "user"
                        };

                        var newIFUser = new InforAccount
                        {
                            InforAccountId = "IDIF" + model.Username,
                            UserId = "IDUs" + model.Username,
                            Email = model.Email.Trim(),
                            Phone = model.Phone
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
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ và hiển thị thông báo lỗi
                ViewBag.Notification = "An error occurred during registration. Please try again later.";
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
        public ActionResult UpdateProfile(string phone, string email, bool sex, string address, string firstname, string lastname)
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
        // Action để hiển thị trang đổi mật khẩu
        public ActionResult ChangePass()
        {
            return View();
        }

        // Action xử lý khi người dùng submit form đổi mật khẩu
        [HttpPost]
        public ActionResult ChangePass(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Lấy UserId từ Session hoặc Identity của người dùng
                var userId = Session["UserId"] as string;

                // Kiểm tra mật khẩu cũ có đúng không, ở đây là ví dụ, bạn cần kiểm tra với dữ liệu thực tế từ cơ sở dữ liệu
                if (IsOldPasswordValid(model.OldPassword))
                {
                    // Kiểm tra mật khẩu mới và mật khẩu xác nhận
                    if (model.NewPassword != model.ConfirmPassword)
                    {
                        // Hiển thị thông báo lỗi nếu mật khẩu xác nhận không trùng
                        ModelState.AddModelError("ConfirmPassword", "Password confirmation does not match the new password.");
                    }
                    else
                    {
                        // Thực hiện logic đổi mật khẩu
                        var user = db.Accounts.FirstOrDefault(u => u.UserId == userId);

                        if (user != null)
                        {
                            // Thực hiện logic đổi mật khẩu, ví dụ:
                            user.Password = model.NewPassword;

                            // Lưu thay đổi vào cơ sở dữ liệu
                            db.SaveChanges();

                            // Hiển thị thông báo đổi mật khẩu thành công
                            ViewBag.Notification = "Password changed successfully!";
                        }
                        else
                        {
                            // Hiển thị thông báo lỗi nếu không tìm thấy người dùng
                            ViewBag.Notification = "User not found.";
                        }
                    }
                }
                else
                {
                    // Hiển thị thông báo lỗi nếu mật khẩu cũ không đúng
                    ModelState.AddModelError("OldPassword", "Old password is incorrect");
                }
            }

            // Nếu có lỗi hoặc thành công, hiển thị lại form đổi mật khẩu với thông báo
            return View(model);
        }


        private bool IsOldPasswordValid(string oldPassword)
        {
            var userId = Session["UserId"] as string;
            if (userId != null)
            {
                // Giả sử có một đối tượng DbContext trong ứng dụng của bạn, ở đây là 'db'
                    // Lấy thông tin người dùng từ cơ sở dữ liệu bằng userId
                    var user = db.Accounts.FirstOrDefault(u => u.UserId == userId);

                    // Kiểm tra xem người dùng tồn tại và mật khẩu cũ đúng hay không
                    if (user != null && oldPassword == user.Password)
                    {
                        // Mật khẩu cũ đúng
                        return true;
                    }
                }

            // Mật khẩu cũ không đúng hoặc có lỗi xảy ra
            return false;
        }

    }
}