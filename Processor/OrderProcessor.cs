using AutoMapper;
using DataAccess.Service;
using DTO;
using Processor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Processor
{
    public class OrderProcessor : IOrderProcessor
    {
        private IOrderDataAccess iOrder;
        public OrderProcessor(IOrderDataAccess iOrder)
        {
            this.iOrder = iOrder;
        }

        public List<PurchaseOrderVM> GetOrderList()
        {
            var dto = iOrder.GetOrderList();
            Mapper.CreateMap<PurchaseOrderDTO, PurchaseOrderVM>();
            return Mapper.Map<List<PurchaseOrderVM>>(dto);
        }

        public string InsertPurchseOrder(PurchaseOrderVM purchaseOrder, List<string> auditChanges)
        {
            Mapper.CreateMap<PurchaseItemsVM, PurchaseItemsDTO>();
            Mapper.CreateMap<PurchaseOrderVM, PurchaseOrderDTO>();
            var dto = Mapper.Map<PurchaseOrderDTO>(purchaseOrder);
            return iOrder.InsertPurchseOrder(dto, auditChanges);
        }

        public List<PurchaseItemsVM> GetPurchaseItems(int PurchaseOrderID)
        {
            var dto = iOrder.GetPurchaseItems(PurchaseOrderID);
            Mapper.CreateMap<PurchaseItemsDTO, PurchaseItemsVM>();
            return Mapper.Map<List<PurchaseItemsVM>>(dto);
        }

        public string UpdatePurchseOrder(PurchaseOrderVM purchaseOrder, List<AuditDetailVM> auditChanges)
        {
            Mapper.CreateMap<PurchaseItemsVM, PurchaseItemsDTO>();
            Mapper.CreateMap<PurchaseOrderVM, PurchaseOrderDTO>();
            var dto = Mapper.Map<PurchaseOrderDTO>(purchaseOrder);

            Mapper.CreateMap<AuditDetailVM, AuditDetailDTO>();
            var auditDTO = Mapper.Map<List<AuditDetailDTO>>(auditChanges);

            return iOrder.UpdatePurchseOrder(dto, auditDTO);
        }
    }
}
