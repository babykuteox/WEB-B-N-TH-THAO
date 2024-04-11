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
    public class loginController : Controller
    {
        private Data db = new Data();

        // GET: Admin/login
        public ActionResult Index()
        {
            if (Session["name"] != null)
            {
                return RedirectToAction("Index", "bills", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("login");
            }

        }

        public ActionResult login()
        {
            return View();
        }

        // POST: Admin/login/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(FormCollection values)
        {
            String user_name = values["user_name"];
            String password = values["password"];
            password = GetMD5(password);
            var acc = db.accounts.Where(s => s.user_name.Equals(user_name) && s.password.Equals(password) && !s.auth.Equals("Khách hàng")).ToList();
            if (acc.Count() > 0)
            {
                if (acc.FirstOrDefault().state != 1)
                {
                    if (user_name.Equals("admin"))
                    {
                        Session["name"] = acc.FirstOrDefault().user_name;
                        Session["auth"] = acc.FirstOrDefault().auth;                      
                    }
                    else
                    {
                        Session["name"] = acc.FirstOrDefault().employees.FirstOrDefault().name;
                        Session["auth"] = acc.FirstOrDefault().auth;
                    }
                    return RedirectToAction("Index");
                }
                else ViewBag.error = "Tài khoản của bạn đã bị khóa vui lòng liên hệ quản trị viên!!";
            }
            else
                ViewBag.error = "Tài khoản hoặc mật khẩu không đúng vui lòng kiểm tra lại!!";
            return View();
        }
        public ActionResult logout()
        {
            if (Session["name"] != null)
            {
                Session["name"] = null;
                Session["auth"] = null;
            }

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
