using Processor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderTaker.Controllers
{
    public class HomeController : Controller
    {
        private ICustomerProcessor iCustomer;
        public HomeController(ICustomerProcessor iCustomer)
        {
            this.iCustomer = iCustomer;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetCustomerList()
        {
            var customer = iCustomer.GetCustomerList();
            return Json(customer, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult InsertCustomer(string FirstName, string LastName, string MobileNumber, string City, bool IsActive, string[] auditChanges)
        {
            var customer = iCustomer.GetCustomerList();
            if(customer.Where(x => x.FullName.ToUpper() == LastName.ToUpper() + ", " + FirstName.ToUpper()).Any())
            {
                return Json("Customer already exist", JsonRequestBehavior.AllowGet);
            }
            if (customer.Where(x => x.MobileNumber == MobileNumber).Any())
            {
                return Json("Mobile number already exist", JsonRequestBehavior.AllowGet);
            }
            string result = iCustomer.InsertCustomer(FirstName, LastName, MobileNumber, City, IsActive, auditChanges);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateCustomer(int ID, string FirstName, string LastName, string MobileNumber, string City, bool IsActive, string[] auditChanges)
        {
            var customer = iCustomer.GetCustomerList().Where(x => x.ID != ID).ToList();
            if (customer.Where(x => x.FullName.ToUpper() == LastName.ToUpper() + ", " + FirstName.ToUpper()).Any())
            {
                return Json("Customer already exist", JsonRequestBehavior.AllowGet);
            }
            if (customer.Where(x => x.MobileNumber == MobileNumber).Any())
            {
                return Json("Mobile number already exist", JsonRequestBehavior.AllowGet);
            }
            string result = iCustomer.UpdateCustomer(ID, FirstName, LastName, MobileNumber, City, IsActive, auditChanges);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}