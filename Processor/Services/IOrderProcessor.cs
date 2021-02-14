using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Processor.Services
{
    public interface IOrderProcessor
    {
        List<PurchaseOrderVM> GetOrderList();
        string InsertPurchseOrder(PurchaseOrderVM purchaseOrder, List<string> auditChanges);
        List<PurchaseItemsVM> GetPurchaseItems(int PurchaseOrderID);
        string UpdatePurchseOrder(PurchaseOrderVM purchaseOrder, List<AuditDetailVM> auditChanges);
    }
}
