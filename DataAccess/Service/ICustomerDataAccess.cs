using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DataAccess.Service
{
    public interface ICustomerDataAccess
    {
        List<CustomerDTO> GetCustomerList();
        string InsertCustomer(string FirstName, string LastName, string MobileNumber, string City, bool IsActive, string[] auditChanges);
        string UpdateCustomer(int ID, string FirstName, string LastName, string MobileNumber, string City, bool IsActive, string[] auditChanges);
        List<CustomerDTO> SearchCustomer(string Keyword);
    }
}
