using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CNWEB.Models;

namespace CNWEB.Areas.Admin.Controllers
{
    public class customersController : Controller
    {
        private Data db = new Data();

        // GET: Admin/customers

        public int pageSize = 10;

        public ActionResult Index(string txtSearch, int? page)
        {
            if (Session["name"] == null)
            {
                return RedirectToAction("login", "login", new { area = "Admin" });
            }
            var data = (from s in db.customers select s);
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

        // GET: Admin/customers/Details/5
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
            customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Admin/customers/Create
        public ActionResult Create()
        {
            if (Session["name"] == null)
            {
                return RedirectToAction("login", "login", new { area = "Admin" });
            }
            return View();
        }

        // POST: Admin/customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection values)
        {
            if (ModelState.IsValid)
            {
                if (values["phone"].Length <= 10)
                {
                    customer cus = new customer();
                    account acc = new account();
                    acc.user_name = values["account.user_name"];
                    acc.password = values["account.user_name"];
                    acc.password = GetMD5(acc.password);
                    acc.auth = values["account.auth"];
                    acc.state = int.Parse(values["account.state"]);

                    db.accounts.Add(acc);
                    if (db.SaveChanges() > 0)
                    {
                        int lastAccountID = db.accounts.OrderByDescending(a => a.id).Select(p => p).FirstOrDefault().id;

                        cus.name = values["name"];
                        cus.gender = values["gender"];
                        cus.phone = int.Parse(values["phone"]);
                        cus.address = values["address"];
                        cus.id_account = lastAccountID;

                        db.customers.Add(cus);
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }
                else
                    ViewBag.error = "Số điện thoại chỉ có 10 ký tự";
            }

            return View(values);
        }

        // GET: Admin/customers/Edit/5
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
            customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Admin/customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection values)
        {
            if (ModelState.IsValid)
            {
                if (values["phone"].Length <= 10)
                {
                    customer customer = db.customers.Find(int.Parse(values["id"]));
                    if (customer == null)
                    {
                        return HttpNotFound();
                    }
                    account account = db.accounts.Find(customer.id_account);
                    if (customer == null)
                    {
                        return HttpNotFound();
                    }

                    account.user_name = values["account.user_name"];
                    account.auth = values["account.auth"];
                    account.state = int.Parse(values["account.state"]);

                    customer.name = values["name"];
                    customer.gender = values["gender"];
                    customer.phone = int.Parse(values["phone"]);
                    customer.address = values["address"];

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                    ViewBag.error = "Số điện thoại chỉ có 10 ký tự";
            }
            return View(values);
        }

        // GET: Admin/customers/Delete/5
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
            customer customer = db.customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Admin/customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            customer customer = db.customers.Find(id);
            db.customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Ban(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            customer customer = db.customers.Find(id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            if (customer.account.state == 0)
                customer.account.state = 1;
            else
                customer.account.state = 0;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public static string GetMD5(string str)
        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            StringBuilder sbHash = new StringBuilder();

            foreach (byte b in bHash)
            {

                sbHash.Append(String.Format("{0:x2}", b));

            }

            return sbHash.ToString();

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
