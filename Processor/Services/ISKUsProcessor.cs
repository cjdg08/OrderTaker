using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Processor.Services
{
    public interface ISKUsProcessor
    {
        List<SKUsVM> GetSKUsList();
        string InsertSKUs(string Name, string Code, double UnitPrice, bool IsActive, string ImagePath, string[] auditChanges);
        string UpdateSKUs(int ID, string Name, string Code, double UnitPrice, bool IsActive, string ImagePath, string[] auditChanges);
    }
}
