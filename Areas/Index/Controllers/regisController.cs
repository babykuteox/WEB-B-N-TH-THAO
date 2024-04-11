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

namespace CNWEB.Areas.Index.Controllers
{
    public class regisController : Controller
    {
        private Data db = new Data();


        public ActionResult Regis()
        {
            var cart = Session["cart"];
            var listcart = new List<cart_item>();
            if (cart != null)
            {
                listcart = (List<cart_item>)cart;
            }
            ViewBag.path = Server.MapPath("~/").Replace("\\", "/");
            ViewBag.listCart = listcart;
            ViewBag.id_account = new SelectList(db.accounts, "id", "user_name");
            return View();
        }

        // POST: Index/regis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Regis(FormCollection values)
        {
            ViewBag.oldValues = values;
            if (values["phone"].Length <= 10)
            {
                if (values["account.password"].Equals(values["confirm_password"]))
                {
                    customer cus = new customer();
                    account account = new account();
                    String user_name = values["account.user_name"];
                    var acc = db.accounts.Where(s => s.user_name.Equals(user_name)).ToList();
                    if (acc.Count == 0)
                    {
                        account = new account();
                        account.user_name = values["account.user_name"];
                        account.password = values["account.password"];
                        account.password = GetMD5(account.password);
                        account.auth = values["account.auth"];
                        account.state = int.Parse(values["account.state"]);

                        db.accounts.Add(account);
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

                            return RedirectToAction("login", "Login", new { area = "Index" });
                        }
                    }
                    else
                    {
                        ViewBag.error = "Đã có tài khoản này trong hệ thống vui lòng chọn tài khoản khác";
                        return View();
                    }
                }
                else
                    ViewBag.error = "Xác nhận mật khẩu không đúng";
            }
            else
                ViewBag.error = "Số điện thoại chỉ có 10 ký tự";
            return View();
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
