using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PurchaseOrderDTO
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public DateTime DateOfDelivery { get; set; }
        public string Status { get; set; }
        public double AmountDue { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserID { get; set; }

        public List<PurchaseItemsDTO> PurchaseItems { get; set; }
    }

    public class PurchaseItemsDTO
    {
        public int ID { get; set; }
        public int PurchaseOrderID { get; set; }
        public int SKUID { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public DateTime TimeStamp { get; set; }
        public string UserID { get; set; }
    }
}
