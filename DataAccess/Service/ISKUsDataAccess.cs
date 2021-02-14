using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public interface ISKUsDataAccess
    {
        List<SKUsDTO> GetSKUsList();
        string InsertSKUs(string Name, string Code, double UnitPrice, bool IsActive, string ImagePath, string[] auditChanges);
        string UpdateSKUs(int ID, string Name, string Code, double UnitPrice, bool IsActive, string ImagePath, string[] auditChanges);
    }
}
