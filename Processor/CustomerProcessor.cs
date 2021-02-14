using DataAccess.Service;
using Processor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using AutoMapper;
using DTO;

namespace Processor
{
    public class CustomerProcessor : ICustomerProcessor
    {
        private ICustomerDataAccess iCustomer;
        public CustomerProcessor(ICustomerDataAccess iCustomer)
        {
            this.iCustomer = iCustomer;
        }

        public List<CustomerVM> GetCustomerList()
        {
            var dto = iCustomer.GetCustomerList();
            Mapper.CreateMap<CustomerDTO, CustomerVM>();
            return Mapper.Map<List<CustomerVM>>(dto);
        }

        public string InsertCustomer(string FirstName, string LastName, string MobileNumber, string City, bool IsActive, string[] auditChanges)
        {
            return iCustomer.InsertCustomer(FirstName, LastName, MobileNumber, City, IsActive, auditChanges);
        }

        public string UpdateCustomer(int ID, string FirstName, string LastName, string MobileNumber, string City, bool IsActive, string[] auditChanges)
        {
            return iCustomer.UpdateCustomer(ID, FirstName, LastName, MobileNumber, City, IsActive, auditChanges);
        }

        public List<CustomerVM> SearchCustomer(string Keyword)
        {
            var dto = iCustomer.SearchCustomer(Keyword);
            Mapper.CreateMap<CustomerDTO, CustomerVM>();
            return Mapper.Map<List<CustomerVM>>(dto);
        }
    }
}
