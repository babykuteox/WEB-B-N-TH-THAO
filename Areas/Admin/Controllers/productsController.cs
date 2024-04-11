using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CNWEB.Models;

namespace CNWEB.Areas.Admin.Controllers
{
    public class productsController : Controller
    {
        private Data db = new Data();

        // GET: Admin/products
        public int pageSize = 10;

        public ActionResult Index(string txtSearch, int? page)
        {
            if (Session["name"] == null)
            {
                return RedirectToAction("login", "login", new { area = "Admin" });
            }
            var data = (from s in db.products select s);
            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                data = data.Where(s => s.name.Contains(txtSearch));
            }

            if (page <= 0 || page == null)
            {
                page = 1;
            }

            int start = (int)(page - 1) * pageSize;

            ViewBag.pageCurrent = page;
            int totalPage = data.Count();
            float totalNumsize = (totalPage / (float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumsize);
            ViewBag.numSize = numSize;
            ViewBag.items = data.OrderByDescending(x => x.id).Skip(start).Take(pageSize);
            ViewBag.path = Server.MapPath("~/").Replace("\\", "/");
            return View();
        }

        // GET: Admin/products/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["name"] == null)
            {
                return RedirectToAction("login", "login", new { area = "Admin" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/products/Create
        public ActionResult Create()
        {
            if (Session["name"] == null)
            {
                return RedirectToAction("login", "login", new { area = "Admin" });
            }
            ViewBag.id_category = new SelectList(db.categories, "id", "name");
            ViewBag.id_manufacturer = new SelectList(db.manufacturers, "id", "name");
            ViewBag.id_unit = new SelectList(db.units, "id", "name");
            return View();
        }


        public String saveImage(String category, String unit, String product, HttpPostedFileBase file, String type)
        {

            String path = Server.MapPath("~/Images/" + Slugify.SlugifyMethod(category));
            if (System.IO.Directory.Exists(path) == false)
            {
                System.IO.Directory.CreateDirectory(path);
            }
            string url_image_main = Server.MapPath("~/Images/" + Slugify.SlugifyMethod(category) + "/" + Slugify.SlugifyMethod(product) + "-" + Slugify.SlugifyMethod(unit) + "-" + type + System.IO.Path.GetExtension(file.FileName));

            if (System.IO.Directory.Exists(url_image_main) == true)
            {
                System.IO.Directory.Delete(url_image_main);
            }

            file.SaveAs(url_image_main);
            return "/Images/" + Slugify.SlugifyMethod(category) + "/" + Slugify.SlugifyMethod(product) + "-" + Slugify.SlugifyMethod(unit) + "-" + type + System.IO.Path.GetExtension(file.FileName);
        }
        // POST: Admin/products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,price,quantity,width,height,detail,image_main,image1,image2,image3,id_category,id_manufacturer,id_unit")] product product, HttpPostedFileBase image_main, HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3)
        {
            category category = db.categories.Find(product.id_category);
            unit unit = db.units.Find(product.id_unit);

            if (ModelState.IsValid)
            {
                if (image_main != null && image_main.ContentLength > 0)
                {
                    product.image_main = saveImage(category.name, unit.name, product.name, image_main, "main");
                }
                if (image1 != null && image1.ContentLength > 0)
                {
                    product.image1 = saveImage(category.name, unit.name, product.name, image1, "1");
                }
                if (image2 != null && image2.ContentLength > 0)
                {
                    product.image2 = saveImage(category.name, unit.name, product.name, image2, "2");
                }
                if (image3 != null && image3.ContentLength > 0)
                {
                    product.image3 = saveImage(category.name, unit.name, product.name, image3, "3");
                }
                db.products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_category = new SelectList(db.categories, "id", "name", product.id_category);
            ViewBag.id_manufacturer = new SelectList(db.manufacturers, "id", "name", product.id_manufacturer);
            ViewBag.id_unit = new SelectList(db.units, "id", "name", product.id_unit);
            return View(product);
        }

        // GET: Admin/products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["name"] == null)
            {
                return RedirectToAction("login", "login", new { area = "Admin" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_category = new SelectList(db.categories, "id", "name", product.id_category);
            ViewBag.id_manufacturer = new SelectList(db.manufacturers, "id", "name", product.id_manufacturer);
            ViewBag.id_unit = new SelectList(db.units, "id", "name", product.id_unit);
            return View(product);
        }

        // POST: Admin/products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,price,quantity,width,height,detail,image1,image2,image3,id_category,id_manufacturer,id_unit")] product product, HttpPostedFileBase image_main, HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3)
        {
            category category = db.categories.Find(product.id_category);
            unit unit = db.units.Find(product.id_unit);
            product pro = db.products.Find(product.id);
            ModelState.Remove("image_main");
            if (ModelState.IsValid)
            {
                if (image_main != null && image_main.ContentLength > 0)
                {
                    pro.image_main = saveImage(category.name, unit.name, product.name, image_main, "main");
                }

                if (image1 != null && image1.ContentLength > 0)
                {
                    pro.image1 = saveImage(category.name, unit.name, product.name, image1, "1");
                }

                if (image2 != null && image2.ContentLength > 0)
                {
                    pro.image2 = saveImage(category.name, unit.name, product.name, image2, "2");
                }

                if (image3 != null && image3.ContentLength > 0)
                {
                    pro.image3 = saveImage(category.name, unit.name, product.name, image3, "3");
                }

                pro.name = product.name;
                pro.price = product.price;
                pro.quantity = product.quantity;
                pro.width = product.width;
                pro.height = product.height;
                pro.detail = product.detail;
                pro.id_category = product.id_category;
                pro.id_manufacturer = product.id_manufacturer;
                pro.id_unit = product.id_unit;

                db.Entry(pro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_category = new SelectList(db.categories, "id", "name", product.id_category);
            ViewBag.id_manufacturer = new SelectList(db.manufacturers, "id", "name", product.id_manufacturer);
            ViewBag.id_unit = new SelectList(db.units, "id", "name", product.id_unit);
            return View(product);
        }

        // GET: Admin/products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["name"] == null)
            {
                return RedirectToAction("login", "login", new { area = "Admin" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            product product = db.products.Find(id);
            
            string url_image_main = Server.MapPath("~" + product.image_main);

            if (System.IO.Directory.Exists(url_image_main) == true && product.image_main != null)
            {
                System.IO.Directory.Delete(url_image_main);
            }

            url_image_main = Server.MapPath("~" + product.image1);

            if (System.IO.Directory.Exists(url_image_main) == true && product.image1 != null)
            {
                System.IO.Directory.Delete(url_image_main);
            }

            url_image_main = Server.MapPath("~" + product.image2);

            if (System.IO.Directory.Exists(url_image_main) == true && product.image2 != null)
            {
                System.IO.Directory.Delete(url_image_main);
            }

            url_image_main = Server.MapPath("~" + product.image3);

            if (System.IO.Directory.Exists(url_image_main) == true && product.image3 != null)
            {
                System.IO.Directory.Delete(url_image_main);
            }

            db.products.Remove(product);
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
