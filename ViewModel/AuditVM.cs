using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class AuditLogsVM
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }

        public List<AuditDetailVM> AuditDetail { get; set; }
    }

    public class AuditDetailVM
    {
        public int ID { get; set; }
        public int AuditLogsID { get; set; }
        public string Details { get; set; }
    }
}
