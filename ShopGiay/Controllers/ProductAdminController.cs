using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ShopGiay.Models;

namespace ShopGiay.Controllers
{
    public class ProductAdminController : Controller
    {
        private Data db = new Data();

        // GET: ProductAdmin
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Account).Include(p => p.ProductCategory);
            return View(products.ToList());
        }

        // GET: ProductAdmin/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: ProductAdmin/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Accounts, "UserId", "Username");
            ViewBag.CategoryId = new SelectList(db.ProductCategories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: ProductAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Price,Quantity,UserId,CategoryId,Descripsion,Image")] Product product, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem có bất kỳ sản phẩm nào trong cơ sở dữ liệu có cùng Name không
                bool isDuplicateName = db.Products.Any(p => p.Name == product.Name);

                if (isDuplicateName)
                {
                    ModelState.AddModelError("Name", "Product name already exists. Please choose another name.");
                }
                else if (imageFile != null && imageFile.ContentLength > 0)
                {
                    // Lưu tên tệp tin hình ảnh
                    string fileName = Path.GetFileName(imageFile.FileName);

                    // Lưu tệp tin hình ảnh vào thư mục
                    string imagePath = Path.Combine(Server.MapPath("~/Public/image/"), fileName);
                    imageFile.SaveAs(imagePath);

                    // Lưu đường dẫn của hình ảnh vào cơ sở dữ liệu
                    product.Image = fileName;


                    // Tạo số ngẫu nhiên
                    Random random = new Random();
                    int randomNumber = random.Next(1000, 9999);
                    //Tạo productId 
                    product.ProductId = $"msp{randomNumber}";

                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {


                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.UserId = new SelectList(db.Accounts, "UserId", "Username", product.UserId);
            ViewBag.CategoryId = new SelectList(db.ProductCategories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: ProductAdmin/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Accounts, "UserId", "Username", product.UserId);
            ViewBag.CategoryId = new SelectList(db.ProductCategories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // POST: ProductAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tên sản phẩm trùng lặp
                if (db.Products.Any(p => p.Name == product.Name && p.ProductId != product.ProductId))
                {
                    ModelState.AddModelError("Name", "Product name already exists.");
                    return View("Edit", product);
                }

                // Kiểm tra tệp tin ảnh đã tải lên
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    // Xóa tệp tin ảnh cũ (nếu tồn tại)
                    string oldImagePath = Server.MapPath("~/Public/image/" + product.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                    // Lưu tệp tin ảnh mới
                    string extension = Path.GetExtension(imageFile.FileName);
                    string newImageName = Guid.NewGuid().ToString() + extension;
                    string newImagePath = Server.MapPath("~/Public/image/" + newImageName);
                    imageFile.SaveAs(newImagePath);

                    // Cập nhật đường dẫn ảnh mới trong sản phẩm
                    product.Image = newImageName;
                }

                // Lưu sản phẩm vào cơ sở dữ liệu
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            // Load UserId và CategoryId để hiển thị trong dropdownlist
            ViewBag.UserId = new SelectList(db.Accounts, "Id", "Name", product.UserId);
            ViewBag.CategoryId = new SelectList(db.ProductCategories, "Id", "Name", product.CategoryId);
            return View("Edit", product);
        }


        // GET: ProductAdmin/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ProductAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
