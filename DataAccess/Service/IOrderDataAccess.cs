using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public interface IOrderDataAccess
    {
        List<PurchaseOrderDTO> GetOrderList();
        string InsertPurchseOrder(PurchaseOrderDTO purchaseOrder, List<string> auditChanges);
        List<PurchaseItemsDTO> GetPurchaseItems(int PurchaseOrderID);
        string UpdatePurchseOrder(PurchaseOrderDTO purchaseOrder, List<AuditDetailDTO> auditChanges);
    }
}
