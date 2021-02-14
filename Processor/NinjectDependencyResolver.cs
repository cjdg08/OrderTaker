using DataAccess.Service;
using Processor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using System.Web.Mvc;
using DataAccess;

namespace Processor
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<ICustomerProcessor>().To<CustomerProcessor>();
            kernel.Bind<ICustomerDataAccess>().To<CustomerDataAccess>();

            kernel.Bind<ISKUsProcessor>().To<SKUsProcessor>();
            kernel.Bind<ISKUsDataAccess>().To<SKUsDataAccess>();

            kernel.Bind<IOrderProcessor>().To<OrderProcessor>();
            kernel.Bind<IOrderDataAccess>().To<OrderDataAccess>();

            kernel.Bind<IAuditProcessor>().To<AuditProcessor>();
            kernel.Bind<IAuditDataAccess>().To<AuditDataAccess>();
        }
    }
}
