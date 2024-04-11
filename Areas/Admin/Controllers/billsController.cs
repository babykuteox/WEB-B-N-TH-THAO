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
    public class billsController : Controller
    {
        private Data db = new Data();

        // GET: Admin/bills
        public int pageSize = 10;

        public ActionResult Index(string txtSearch, int? page, string fillter)
        {
            if (Session["name"] == null)
            {
                return RedirectToAction("login", "login", new { area = "Admin" });
            }

            var data = (from s in db.bills select s);
            if(!String.IsNullOrEmpty(fillter))
            {
                ViewBag.fillter = fillter;
                data = data.Where(s => s.state.ToString().Contains(fillter));
            }
            if (!String.IsNullOrEmpty(txtSearch))
            {
                ViewBag.txtSearch = txtSearch;
                data = data.Where(s => s.id.ToString().Contains(txtSearch));
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

        // GET: Admin/bills/Details/5
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
            bill bill = db.bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // GET: Admin/bills/Create
        public ActionResult Create()
        {
            if (Session["name"] == null)
            {
                return RedirectToAction("login", "login", new { area = "Admin" });
            }

            ViewBag.id_customer = new SelectList(db.customers, "id", "name");
            return View();
        }

        // POST: Admin/bills/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,date_create,total_price,address,note,id_customer")] bill bill)
        {
            if (ModelState.IsValid)
            {
                db.bills.Add(bill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_customer = new SelectList(db.customers, "id", "name", bill.id_customer);
            return View(bill);
        }

        // GET: Admin/bills/Edit/5
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
            bill bill = db.bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_customer = new SelectList(db.customers, "id", "name", bill.id_customer);
            return View(bill);
        }

        // POST: Admin/bills/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,date_create,total_price,address,note,id_customer")] bill bill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_customer = new SelectList(db.customers, "id", "name", bill.id_customer);
            return View(bill);
        }

        // GET: Admin/bills/Delete/5
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
            bill bill = db.bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // POST: Admin/bills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bill bill = db.bills.Find(id);
            db.bills.Remove(bill);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult stateChange(FormCollection values)
        {
            if (int.Parse(values["id"]) == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            bill bill = db.bills.Find(int.Parse(values["id"]));

            if (bill == null)
            {
                return HttpNotFound();
            }

            bill.state = int.Parse(values["state"]);
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
