using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public interface IAuditDataAccess
    {
        List<AuditLogsDTO> GetAuditLogs();
        List<AuditDetailDTO> GetAuditDetail(int AuditLogsID);
    }
}
