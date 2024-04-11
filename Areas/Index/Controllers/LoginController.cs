using CNWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CNWEB.Areas.Index.Controllers
{
    public class LoginController : Controller
    {
        private Data db = new Data();

        // GET: Admin/login
        public ActionResult Index()
        {
            if (Session["id_user"] != null)
            {
                return RedirectToAction("Index", "Index", new { area = "Index" });
            }
            else
            {
                return RedirectToAction("login");
            }

        }


        public ActionResult login()
        {
            var cart = Session["cart"];
            var listcart = new List<cart_item>();
            if (cart != null)
            {
                listcart = (List<cart_item>)cart;
            }
            ViewBag.path = Server.MapPath("~/").Replace("\\", "/");
            ViewBag.listCart = listcart;
            return View("~/Areas/Index/Views/Home/Login.cshtml");
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
            var acc = db.accounts.Where(s => s.user_name.Equals(user_name) && s.password.Equals(password) && !s.auth.Equals("Admin") && !s.auth.Equals("Nhân Viên")).ToList();
            if (acc.Count() > 0)
            {
                if (acc.FirstOrDefault().state != 1)
                {
                    Session["id_user"] = acc.FirstOrDefault().customers.FirstOrDefault().id;
                    return RedirectToAction("Index");
                }
                else ViewBag.error = "Tài khoản của bạn đã bị khóa vui lòng liên hệ quản trị viên!!";
            }
            else
                ViewBag.error = "Tài khoản hoặc mật khẩu không đúng vui lòng kiểm tra lại!!";
            var cart = Session["cart"];
            var listcart = new List<cart_item>();
            if (cart != null)
            {
                listcart = (List<cart_item>)cart;
            }
            ViewBag.path = Server.MapPath("~/").Replace("\\", "/");
            ViewBag.listCart = listcart;
            return View("~/Areas/Index/Views/Home/Login.cshtml");
        }

        public ActionResult logout()
        {
            if (Session["id_user"] != null)
            {
                Session["id_user"] = null;
            }

            return RedirectToAction("Index", "Index", new { area = "Index" });
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