using GeneralStoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GeneralStoreMVC.Controllers
{
    public class TransactionController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Transaction
        public ActionResult Index()
        {
            var transactions = _db.Transactions.Include(t => t.Customer).Include(t => t.Product).ToList();
            return View(transactions);
        }

        // GET: Transaction/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(_db.Customers, "CustomerID", "FullName");
            ViewBag.ProductID = new SelectList(_db.Products, "ProductID", "Name");
            return View();
        }

        // POST: Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var product = _db.Products.Find(transaction.ProductID);
                if (product.InventoryCount - transaction.Quantity < 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // Not the best way, could use error message to say out of stock
                }

                product.InventoryCount -= transaction.Quantity; // Removes 1 from InventoryCount when a product is bought

                _db.Transactions.Add(transaction);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(_db.Customers, "CustomerID", "FullName", transaction.CustomerID);
            ViewBag.ProductID = new SelectList(_db.Products, "ProductID", "Name", transaction.ProductID);
            return View(transaction);
        }

        // GET: Transaction/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = _db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transaction/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = _db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(_db.Customers, "CustomerID", "FullName", transaction.CustomerID);
            ViewBag.ProductID = new SelectList(_db.Products, "ProductID", "Name", transaction.ProductID);
            return View(transaction);
        }

        // POST: Transaction/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(transaction).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(_db.Customers, "CustomerID", "FullName", transaction.CustomerID);
            ViewBag.ProductID = new SelectList(_db.Products, "ProductID", "Name", transaction.ProductID);
            return View(transaction);
        }

        // GET: Transaction/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = _db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transaction/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = _db.Transactions.Find(id);
            _db.Transactions.Remove(transaction);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}