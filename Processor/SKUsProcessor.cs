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
    public class SKUsProcessor : ISKUsProcessor
    {
        private ISKUsDataAccess iSKUs;
        public SKUsProcessor(ISKUsDataAccess iSKUs)
        {
            this.iSKUs = iSKUs;
        }

        public List<SKUsVM> GetSKUsList()
        {
            var dto = iSKUs.GetSKUsList();
            Mapper.CreateMap<SKUsDTO, SKUsVM>();
            return Mapper.Map<List<SKUsVM>>(dto);
        }

        public string InsertSKUs(string Name, string Code, double UnitPrice, bool IsActive, string ImagePath, string[] auditChanges)
        {
            return iSKUs.InsertSKUs(Name, Code, UnitPrice, IsActive, ImagePath, auditChanges);
        }

        public string UpdateSKUs(int ID, string Name, string Code, double UnitPrice, bool IsActive, string ImagePath, string[] auditChanges)
        {
            return iSKUs.UpdateSKUs(ID, Name, Code, UnitPrice, IsActive, ImagePath, auditChanges);
        }
    }
}
