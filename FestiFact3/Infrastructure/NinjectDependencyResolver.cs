using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using Model.Abstract;
using Model.Concrete;
using System.Web.Mvc;

namespace FestiFact3.Infrastructure
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
            kernel.Bind<IFestivalRepository>().To<EFFestivalRepository>();
        }
    }
}