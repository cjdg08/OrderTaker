using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Processor.Services
{
    public interface ICustomerProcessor
    {
        List<CustomerVM> GetCustomerList();
        string InsertCustomer(string FirstName, string LastName, string MobileNumber, string City, bool IsActive, string[] auditChanges);
        string UpdateCustomer(int ID, string FirstName, string LastName, string MobileNumber, string City, bool IsActive, string[] auditChanges);
        List<CustomerVM> SearchCustomer(string Keyword);
    }
}
