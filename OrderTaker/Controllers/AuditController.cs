using Processor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderTaker.Controllers
{
    public class AuditController : Controller
    {
        private IAuditProcessor iAudit;
        public AuditController(IAuditProcessor iAudit)
        {
            this.iAudit = iAudit;
        }
        // GET: Audit
        public ActionResult Index()
        {
            var auditLogs = iAudit.GetAuditLogs();
            return View(auditLogs);
        }

        public JsonResult GetAuditDetail(int AuditLogsID)
        {
            var data = iAudit.GetAuditDetail(AuditLogsID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}