using Processor.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderTaker.Controllers
{
    public class SKUsController : Controller
    {
        private ISKUsProcessor iSKUs;
        public SKUsController(ISKUsProcessor iSKUs)
        {
            this.iSKUs = iSKUs;
        }

        // GET: SKUs
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetSKUsLIst()
        {
            var SKUs = iSKUs.GetSKUsList();
            return Json(SKUs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult InsertSKUs(string Name, string Code, double UnitPrice, bool IsActive, string[] auditChanges)
        {
            var SKUs = iSKUs.GetSKUsList();
            if (SKUs.Where(x => x.Name.ToUpper() == Name.ToUpper()).Any())
            {
                return Json("Name already exist", JsonRequestBehavior.AllowGet);
            }

            if (SKUs.Where(x => x.Code.ToUpper() == Code.ToUpper()).Any())
            {
                return Json("Code already exist", JsonRequestBehavior.AllowGet);
            }

            string fileExtension = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {
                fileExtension = Path.GetExtension(Request.Files[i].FileName);
            }

            string ImagePath = Request.Files.Count >= 1 ? "/SKUs Images/" + Code + fileExtension : "";

            List<string> list = new List<string>();
            list.Add("Image Path=" + ImagePath);
            foreach(string txt in auditChanges)
            {
                list.Add(txt);
            }

            string result = iSKUs.InsertSKUs(Name, Code, UnitPrice, IsActive, ImagePath, list.ToArray());

            if(result == "Success")
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    fileExtension = Path.GetExtension(Request.Files[i].FileName);
                    Request.Files[i].SaveAs(Server.MapPath("/SKUs Images/" + Code + fileExtension));
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateSKUs(int ID, string Name, string Code, double UnitPrice, bool IsActive, string[] auditChanges)
        {
            var SKUs = iSKUs.GetSKUsList().Where(x => x.ID != ID).ToList();
            if (SKUs.Where(x => x.Name.ToUpper() == Name.ToUpper()).Any())
            {
                return Json("Name already exist", JsonRequestBehavior.AllowGet);
            }

            if (SKUs.Where(x => x.Code.ToUpper() == Code.ToUpper()).Any())
            {
                return Json("Code already exist", JsonRequestBehavior.AllowGet);
            }

            string fileExtension = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {
                fileExtension = Path.GetExtension(Request.Files[i].FileName);
            }

            string ImagePath = Request.Files.Count >= 1 ? "/SKUs Images/" + Code + fileExtension : "";

            string result = iSKUs.UpdateSKUs(ID, Name, Code, UnitPrice, IsActive, ImagePath, auditChanges);

            if (result == "Success")
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    fileExtension = Path.GetExtension(Request.Files[i].FileName);
                    Request.Files[i].SaveAs(Server.MapPath("/SKUs Images/" + Code + fileExtension));
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}