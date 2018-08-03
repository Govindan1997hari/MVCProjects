using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject.Models;
namespace MVCProject.Controllers
{
    public class OrderCRUDController : Controller
    {
        ProjectDBEntities db = new ProjectDBEntities();
        [HttpGet]
        public ActionResult AddOrder()
        {
            var data = new SelectList(db.tbl_customer, "customerId", "customerName");
            Session["sdata"] = data;
            return View();
        }
        [HttpPost]
        public ActionResult AddOrder(string command)
        {
            if (command == "Insert")
            {
                tbl_order obj = new tbl_order();
                obj.productName = Request.Form["txtpname"].ToString();
                obj.price = decimal.Parse(Request.Form["txtprice"].ToString());
                obj.customerId = int.Parse(Request.Form["ddlname"].ToString());
                db.tbl_order.Add(obj);
                var result = db.SaveChanges();
                if (result > 0)
                {
                    ModelState.AddModelError("", "Order Placed Successfully");
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Technical Error Occured:Unable to Place Order !");
                    return View();
                }
            }
            else
            {
                
                return View();
            }
        }
        [HttpGet]
        public ActionResult GetOrderByCustomer()
        {
            var data = new SelectList(db.tbl_customer, "customerId", "customerName");
            Session["sdata"] = data;
            return View();
        }
        [HttpPost]
        public ActionResult GetOrderByCustomer(string command)
        {
            int custid = int.Parse(Request.Form["ddlid"].ToString());
            if (command == "Search")
            {
                var data = (from t in db.tbl_order
                            where t.customerId == custid
                            select t).ToList();
                Session["pdata"] = data;
                return View();
            }
            return View();
        }
        [HttpGet]
        public ActionResult GetOrderByID()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetOrderByID(string command)
        {
            int custid = int.Parse(Request.Form["txtid"].ToString());
            if (command == "Search")
            {
                var data = (from t in db.tbl_order
                            where t.customerId == custid
                            select t).ToList();
                Session["pdata"] = data;
                return View();
            }
            return View();
        }
        [HttpGet]
        public ActionResult ChangeOrderPrice(int id)
        {
            var data = (from t in db.tbl_order
                        where t.orderId == id
                        select t).SingleOrDefault();
            Session["udata"] = data;
            return View();
        }
        [HttpPost]
        public ActionResult ChangeOrderPrice()
        {
            int id = int.Parse(Request.Form["txtid"].ToString());
            var olddata = db.tbl_order.Where(x => x.orderId == id).SingleOrDefault();
            olddata.price = decimal.Parse(Request.Form["txtprice"].ToString());
            var res = db.SaveChanges();
            if (Session["check"].ToString() == "customer")
                return RedirectToAction("GetOrderByCustomer");
            else
                return RedirectToAction("GetOrderByID");

        }
        [HttpGet]
        public ActionResult Homepage()
        {
            return View();
        }
    }
}