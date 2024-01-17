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
    public class AccountManagementController : Controller
    {
        private Data db = new Data();

        // GET: AccountManagement
        public ActionResult Index()
        {
            return View(db.Accounts.ToList());
        }

        // GET: AccountManagement/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: AccountManagement/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,Password,Role")] Account account)
        {
            // Kiểm tra xem UserId đã tồn tại hay chưa
            if (db.Accounts.Any(a => a.UserId == account.UserId))
            {
                ModelState.AddModelError("UserId", "UserId already exists.");
            }

            if (ModelState.IsValid)
            {

                account.UserId = "IDUs" + account.Role + account.Username;
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(account);
        }

        // GET: AccountManagement/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: AccountManagement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Username,Password,Role")] Account updatedAccount)
        {
            if (ModelState.IsValid)
            {
                // Find the existing account in the database
                var existingAccount = db.Accounts.Find(updatedAccount.UserId);

                if (existingAccount == null)
                {
                    return HttpNotFound(); // or handle the situation where the account is not found
                }

                // Update properties of the existing account
                existingAccount.Username = updatedAccount.Username;
                existingAccount.Password = updatedAccount.Password;
                existingAccount.Role = updatedAccount.Role;

                // Save changes to the database
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            // If ModelState is not valid, return to the Edit view with the model
            return View(updatedAccount);
        }

        // GET: AccountManagement/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: AccountManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            // Xóa các bản ghi liên quan trong bảng "Product"
            var products = db.Products.Where(p => p.UserId == id).ToList();
            db.Products.RemoveRange(products);

            // Xóa các bản ghi liên quan trong bảng "Cart"
            var carts = db.Carts.Where(c => c.UserId == id).ToList();
            db.Carts.RemoveRange(carts);
            // Xóa các bản ghi liên quan trong bảng "InforAccount"
            var inforAccounts = db.InforAccounts.Where(i => i.UserId == id).ToList();
            db.InforAccounts.RemoveRange(inforAccounts);

            // Xóa tài khoản
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            db.Accounts.Remove(account);
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
