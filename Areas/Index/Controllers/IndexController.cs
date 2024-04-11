using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CNWEB.Models;

namespace CNWEB.Areas.Index.Controllers
{
    public class IndexController : Controller
    {
        private Data db = new Data();

        // GET: Admin/bills
        public int pageSize = 10;

        public ActionResult Index(string txtSearch, int? page, string fillter)
        {

            List<category> categories = db.categories.ToList();
            var cart = Session["cart"];
            var listcart = new List<cart_item>();
            if (cart != null)
            {
                listcart = (List<cart_item>)cart;
            }

            var data = (from s in db.products select s);
            if (!String.IsNullOrEmpty(fillter))
            {
                ViewBag.fillter = fillter;
                data = data.Where(s => s.category.name.ToString().Contains(fillter));
            }
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
            ViewBag.listCart = listcart;
            ViewBag.categories = categories;
            return View("~/Areas/Index/Views/Home/Index1.cshtml");
        }

        public ActionResult Detail(int? id)
        {
            var cart = Session["cart"];
            var listcart = new List<cart_item>();
            if (cart != null)
            {
                listcart = (List<cart_item>)cart;
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
            ViewBag.item = product;
            ViewBag.path = Server.MapPath("~/").Replace("\\", "/");
            ViewBag.listCart = listcart;
            return View("~/Areas/Index/Views/Home/Detail.cshtml");
        }

        public ActionResult Account()
        {

            var cart = Session["cart"];
            var listcart = new List<cart_item>();
            if (cart != null)
            {
                listcart = (List<cart_item>)cart;
            }
       
            if (Session["id_user"] == null)
            {
                return RedirectToAction("login", "Login", new { area = "Index" });
            }
            int id = int.Parse(Session["id_user"].ToString());
            var customer = db.customers.Find(Session["id_user"]);
            var bills = db.bills.Where(s => s.id_customer == id).OrderByDescending(x => x.id).ToList();  

            ViewBag.bills = bills;
            ViewBag.customer = customer;
            ViewBag.path = Server.MapPath("~/").Replace("\\", "/");
            ViewBag.listCart = listcart;
            return View("~/Areas/Index/Views/Home/MyAccount.cshtml");
        }

        public ActionResult CheckOut()
        {
            var customer = new customer();
            if (Session["id_user"] != null)
            {
                customer = db.customers.Find(Session["id_user"]);
            }
            else
            {
                return RedirectToAction("login", "Login", new { area = "Index" });
            }

            var cart = Session["cart"];
            if (cart == null)
            {
                return View("~/Areas/Index/Views/Home/CartEmpty.cshtml");
            }

            
            var listcart = new List<cart_item>();
            if (cart != null)
            {
                listcart = (List<cart_item>)cart;
            }
            ViewBag.customer = customer;
            ViewBag.path = Server.MapPath("~/").Replace("\\", "/");
            ViewBag.listCart = listcart;
            return View("~/Areas/Index/Views/Home/CheckOut.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(String address, String note, double total)
        {
            DateTime today = DateTime.Today;
            bill bill = new bill();
            bill_detail bill_detail;
            var cart = Session["cart"];
            var listcart = new List<cart_item>();

            if (cart != null)
            {
                bill.date_create = today;
                bill.total_price = total;
                bill.address = address;
                bill.note = note;
                bill.state = 0;
                bill.id_customer = int.Parse(Session["id_user"].ToString());
                db.bills.Add(bill);
                if (db.SaveChanges() > 0)
                {
                    int lastBillID = db.bills.OrderByDescending(a => a.id).Select(p => p).FirstOrDefault().id;

                    listcart = (List<cart_item>)cart;
                    foreach(cart_item item in listcart)
                    {
                        bill_detail = new bill_detail();
                        bill_detail.id_bill = lastBillID;
                        bill_detail.id_product = item.product.id;
                        bill_detail.quantity = item.quantity;
                        db.bill_detail.Add(bill_detail);
                        db.SaveChanges();
                    }
                    Session["cart"] = null;
                }
            }
            return View("~/Areas/Index/Views/Home/OrderCompleted.cshtml");
        }

        public ActionResult Cart()
        {
            var cart = Session["cart"];
            var listcart = new List<cart_item>();
            if (cart != null)
            {
                listcart = (List<cart_item>)cart;
            }

            ViewBag.path = Server.MapPath("~/").Replace("\\", "/");
            ViewBag.listCart = listcart;
            return View("~/Areas/Index/Views/Home/Cart.cshtml");
        }

        public ActionResult Add(int id, int quantity, string view)
        {
            var pro = db.products.Find(id);
            var cart = Session["cart"];
            if (cart == null)
            {
                var item = new cart_item();
                item.product = pro;
                item.quantity = quantity;
                var list = new List<cart_item>();
                list.Add(item);
                Session["cart"] = list;
            }
            else
            {
                var list = (List<cart_item>)cart;
                if (list.Exists(x => x.product.id == id))
                {
                    foreach (var item in list)
                    {
                        if (item.product.id == id)
                        {
                            item.quantity += quantity;
                        }
                    }
                }
                else
                {
                    var item = new cart_item();
                    item.product = pro;
                    item.quantity = quantity;
                    list.Add(item);
                }
            }
            if (view.Equals("detail"))
                return RedirectToAction("Detail", new { id = id });
            if (view.Equals("cart"))
                return RedirectToAction("Cart");
            return RedirectToAction("Index");
        }

        public ActionResult Remove(int id, string view)
        {
            var pro = db.products.Find(id);
            var cart = Session["cart"];

            var list = (List<cart_item>)cart;
            if (list.Exists(x => x.product.id == id))
            {

                foreach (var item in list)
                {
                    if (item.product.id == id)
                    {
                        list.Remove(item);
                        break;
                    }

                }
            }
            if (view.Equals("detail"))
                return RedirectToAction("Detail", new { id = id });
            if (view.Equals("cart"))
                return RedirectToAction("Cart");
            return RedirectToAction("Index");
        }

        public ActionResult Sub(int id, int quantity, string view)
        {
            var pro = db.products.Find(id);
            var cart = Session["cart"];

            var list = (List<cart_item>)cart;
            if (list.Exists(x => x.product.id == id))
            {

                foreach (var item in list)
                {
                    if (item.product.id == id)
                    {
                        item.quantity -= quantity;
                    }

                }
            }
            if (view.Equals("detail"))
                return RedirectToAction("Detail", new { id = id });
            if (view.Equals("cart"))
                return RedirectToAction("Cart");
            return RedirectToAction("Index");
        }
    }
}