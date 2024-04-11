using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CNWEB.Models;

namespace CNWEB.Areas.Admin.Controllers
{
    public class unitsController : Controller
    {
        private Data db = new Data();

        // GET: Admin/units
        public int pageSize = 10;

        public ActionResult Index(string txtSearch, int? page)
        {
            if (Session["name"] == null)
            {
                return RedirectToAction("login", "login", new { area = "Admin" });
            }
            var data = (from s in db.units select s);
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

            return View();
        }

        // GET: Admin/units/Details/5
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
            unit unit = db.units.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        // GET: Admin/units/Create
        public ActionResult Create()
        {
            if (Session["name"] == null)
            {
                return RedirectToAction("login", "login", new { area = "Admin" });
            }
            return View();
        }

        // POST: Admin/units/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name")] unit unit)
        {
            if (ModelState.IsValid)
            {
                db.units.Add(unit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(unit);
        }

        // GET: Admin/units/Edit/5
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
            unit unit = db.units.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        // POST: Admin/units/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name")] unit unit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unit);
        }

        // GET: Admin/units/Delete/5
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
            unit unit = db.units.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        // POST: Admin/units/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            unit unit = db.units.Find(id);
            db.units.Remove(unit);
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
