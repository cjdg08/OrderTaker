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
    public class AuditProcessor : IAuditProcessor
    {
        private IAuditDataAccess iAudit;
        public AuditProcessor(IAuditDataAccess iAudit)
        {
            this.iAudit = iAudit;
        }

        public List<AuditLogsVM> GetAuditLogs()
        {
            var dto = iAudit.GetAuditLogs();
            Mapper.CreateMap<AuditLogsDTO, AuditLogsVM>();
            return Mapper.Map<List<AuditLogsVM>>(dto);
        }

        public List<AuditDetailVM> GetAuditDetail(int AuditLogsID)
        {
            var dto = iAudit.GetAuditDetail(AuditLogsID);
            Mapper.CreateMap<AuditDetailDTO, AuditDetailVM>();
            return Mapper.Map<List<AuditDetailVM>>(dto);
        }
    }
}
