using Processor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModel;

namespace OrderTaker.Controllers
{
    public class OrderController : Controller
    {
        private IOrderProcessor iOrder;
        private ICustomerProcessor iCustomer;
        private ISKUsProcessor iSKUs;
        public OrderController(IOrderProcessor iOrder, ICustomerProcessor iCustomer, ISKUsProcessor iSKUs)
        {
            this.iOrder = iOrder;
            this.iCustomer = iCustomer;
            this.iSKUs = iSKUs;
        }

        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetOrderList()
        {
            var order = iOrder.GetOrderList();
            return Json(order, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateOrder()
        {
            var SKUs = iSKUs.GetSKUsList().Where(x => x.IsActive == true).ToList();
            List<SelectListItem> ddlItem = new List<SelectListItem>();
            ddlItem.Add(new SelectListItem
            {
                Value = "",
                Text = "- Please Select -"
            });
            foreach (var item in SKUs) {
                ddlItem.Add(new SelectListItem
                {
                    Value = item.ID.ToString(),
                    Text = item.Name
                });
            }
            ViewBag.ddlItem = ddlItem;
            return View();
        }

        public JsonResult SearchCustomer(string Keyword)
        {
            var customer = iCustomer.SearchCustomer(Keyword);
            return Json(customer, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUnitPrice(int ID)
        {
            double unitPrice = iSKUs.GetSKUsList().Where(x => x.ID == ID).FirstOrDefault().UnitPrice;
            return Json(unitPrice, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEditItemDDL(int[] IDs, int editingID)
        {
            var SKUs = iSKUs.GetSKUsList().Where(x => !IDs.Contains(x.ID) && x.IsActive == true).ToList();
            var editingItem = iSKUs.GetSKUsList().Where(x => x.ID == editingID).FirstOrDefault();
            List<SelectListItem> ddlItem = new List<SelectListItem>();
            ddlItem.Add(new SelectListItem
            {
                Value = "",
                Text = "- Please Select -"
            });
            if (editingItem != null)
            {
                ddlItem.Add(new SelectListItem
                {
                    Value = editingItem.ID.ToString(),
                    Text = editingItem.Name,
                    Selected = true
                });
            }
            foreach (var item in SKUs)
            {
                ddlItem.Add(new SelectListItem
                {
                    Value = item.ID.ToString(),
                    Text = item.Name
                });
            }

            return Json(ddlItem, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult InsertOrder(string FullName, DateTime DateOfDeliver, string Status, PurchaseItemsVM[] purchaseItem, double amountDue)
        {
            var customer = iCustomer.GetCustomerList().Where(x => x.FullName == FullName).FirstOrDefault();
            PurchaseOrderVM purchaseOrder = new PurchaseOrderVM();

            if (customer == null)
            {
                return Json("Customer does not exist", JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (customer.IsActive)
                {
                    purchaseOrder.CustomerID = customer.ID;
                    purchaseOrder.CustomerName = customer.FullName;
                    purchaseOrder.DateOfDelivery = DateOfDeliver;
                    purchaseOrder.AmountDue = amountDue;
                    purchaseOrder.Status = Status;
                }
                else
                {
                    return Json("Customer is not active", JsonRequestBehavior.AllowGet);
                }
            }

            List<string> auditChanges = new List<string>();
            auditChanges.Add("Customer ID=\"" + purchaseOrder.CustomerID.ToString() + "\"");
            auditChanges.Add("Customer Name=\"" + purchaseOrder.CustomerName + "\"");
            auditChanges.Add("Date of Deliver=\"" + purchaseOrder.DateOfDelivery.ToString() + "\"");
            auditChanges.Add("Status=\"" + purchaseOrder.Status + "\"");

            List<PurchaseItemsVM> PurchaseItem = new List<PurchaseItemsVM>();
            foreach(var item in purchaseItem)
            {
                PurchaseItem.Add(item);
                auditChanges.Add("SKU ID=\"" + item.SKUID + "\", Quantity=\"" + item.Quantity + "\"");
            }

            purchaseOrder.PurchaseItems = PurchaseItem;
            

            string result = iOrder.InsertPurchseOrder(purchaseOrder, auditChanges);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditOrder(int ID)
        {
            var purchaseOrder = iOrder.GetOrderList().Where(x => x.ID == ID).FirstOrDefault();
            var purchaseItems = iOrder.GetPurchaseItems(purchaseOrder.ID);
            purchaseOrder.PurchaseItems = purchaseItems;

            var IDs = purchaseOrder.PurchaseItems.Select(x => x.SKUID).ToArray();
            var SKUs = iSKUs.GetSKUsList().Where(x => !IDs.Contains(x.ID) && x.IsActive == true).ToList();

            List<SelectListItem> ddlItem = new List<SelectListItem>();
            ddlItem.Add(new SelectListItem
            {
                Value = "",
                Text = "- Please Select -"
            });
            foreach (var item in SKUs)
            {
                ddlItem.Add(new SelectListItem
                {
                    Value = item.ID.ToString(),
                    Text = item.Name
                });
            }
            ViewBag.ddlItem = ddlItem;

            return View(purchaseOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateOrder(int ID, string FullName, DateTime DateOfDeliver, string Status, PurchaseItemsVM[] purchaseItem, double amountDue, AuditDetailVM[] auditChanges)
        {
            var customer = iCustomer.GetCustomerList().Where(x => x.FullName == FullName).FirstOrDefault();
            PurchaseOrderVM purchaseOrder = new PurchaseOrderVM();

            if (customer == null)
            {
                return Json("Customer does not exist", JsonRequestBehavior.AllowGet);
            }
            else
            {
                purchaseOrder.ID = ID;
                purchaseOrder.CustomerID = customer.ID;
                purchaseOrder.CustomerName = customer.FullName;
                purchaseOrder.DateOfDelivery = DateOfDeliver;
                purchaseOrder.AmountDue = amountDue;
                purchaseOrder.Status = Status;
            }

            List<PurchaseItemsVM> PurchaseItem = new List<PurchaseItemsVM>();
            foreach (var item in purchaseItem)
            {
                item.PurchaseOrderID = ID;
                PurchaseItem.Add(item);
            }

            List<AuditDetailVM> auditDetails = new List<AuditDetailVM>();
            foreach (var item in auditChanges)
            {
                auditDetails.Add(item);
            }

            purchaseOrder.PurchaseItems = PurchaseItem;

            string result = iOrder.UpdatePurchseOrder(purchaseOrder, auditDetails);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}