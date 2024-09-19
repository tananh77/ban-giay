using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopGiay.Models;

namespace ShopGiay.Controllers
{
    public class ProductCategoriesAdminController : Controller
    {
        private Data db = new Data();

        // GET: ProductCategoriesAdmin
        public ActionResult Index()
        {
            var list = db.ProductCategories.ToList();
            return View("Index", list);
        }

        // GET: ProductCategoriesAdmin/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // GET: ProductCategoriesAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductCategoriesAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryName")] ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem có loại sản phẩm có cùng Name đã tồn tại trong cơ sở dữ liệu hay không
                var existingCategory = db.ProductCategories.FirstOrDefault(c => c.CategoryName == productCategory.CategoryName);
                if (existingCategory != null)
                {
                    ModelState.AddModelError("", "The product type name already exists.");
                    return View(productCategory);
                }

                // Tạo số ngẫu nhiên
                Random random = new Random();
                int randomNumber = random.Next(1000, 9999);


                // Gán productcategoryId ngẫu nhiên
                productCategory.CategoryId = $"madanhmuc{randomNumber}";

                db.ProductCategories.Add(productCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productCategory);
        }

        // GET: ProductCategoriesAdmin/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // POST: ProductCategoriesAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryName")] ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem có loại sản phẩm khác có cùng tên đã tồn tại trong cơ sở dữ liệu hay không
                var existingCategory = db.ProductCategories.FirstOrDefault(c => c.CategoryName == productCategory.CategoryName);
                if (existingCategory != null)
                {
                    ModelState.AddModelError("", " The product type name already exists.");
                    return View(productCategory);
                }

                db.Entry(productCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productCategory);
        }

        // GET: ProductCategoriesAdmin/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // POST: ProductCategoriesAdmin/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }

            // Xóa các bản ghi liên quan trong bảng "Products"
            var products = db.Products.Where(p => p.CategoryId == id).ToList();
            db.Products.RemoveRange(products);

            db.ProductCategories.Remove(productCategory);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
