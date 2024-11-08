using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using lapTrinhWebTH.Models;
using PagedList;

namespace lapTrinhWebTH.Controllers
{
    public class ProductsController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        // GET: Products
        public ActionResult ProductList(int? category, int? page, string SearchString, double min = double.MinValue,double max = double.MaxValue)
        { // Tạo bộ mẫu tin Products và có tham chiếu đến Category
            var products = db.Products.Include(p => p.Category);
            // Tìm kiếm chuỗi truy vấn theo category
            if (category == null)
            {
                products = db.Products.OrderByDescending(x => x.ProductName);
            }
            else
            {
                products = db.Products.OrderByDescending(x => x.ProductName).Where(x => x.CategoryID ==
               category);
            }
            //Tìm kiếm chuỗi truy vấn theo ProductName, nếu chuỗi truy vấn SearchString khác rỗng, null
            if (!String.IsNullOrEmpty(SearchString))
            {
                products = products.Where(s => s.ProductName.Contains(SearchString));
            }
            // Tìm kiếm chuỗi truy vấn theo đơn giá
            if (min >= 0 && max > 0)
            {
                products = db.Products.OrderByDescending(x => x.ProductPrice).Where(p =>
               (double)p.ProductPrice >= min && (double)p.ProductPrice <= max);
            }
            // Khai báo mỗi trang 4 sản phẩm
            int pageSize = 4;
            // Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);
            // Nếu page = null thì đặt lại page là 1.
            if (page == null) page = 1;
            // Trả về các product được phân trang theo kích thước và số trang.
            return View(products.ToPagedList(pageNumber, pageSize));

        }
        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
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

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,ProductDecription,CategoryID,ProductPrice,ProductImage")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,ProductDecription,CategoryID,ProductPrice,ProductImage")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
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
