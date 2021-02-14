using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Processor.Services
{
    public interface IAuditProcessor
    {
        List<AuditLogsVM> GetAuditLogs();
        List<AuditDetailVM> GetAuditDetail(int AuditLogsID);
    }
}
