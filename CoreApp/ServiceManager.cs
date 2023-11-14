using DataAccess.CRUD;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp
{
    public class ServiceManager
    {
        List<Service> list = new List<DTOs.Service>();
        public ServiceManager()
        {

            var serviceCrud = new ServiceCrudFactory();
            this.list = serviceCrud.RetrieveAll();
        }

        public bool IsUnique(string serviceName)
        {
            bool unique = true;
            foreach (var service in list)
            {
                if(service.ServiceName == serviceName)
                {
                    unique = false; break;
                }
            }
            return unique;
        }

        public void Create(Service service)
        {
            if(!IsUnique(service.ServiceName))
            {
              throw new Exception("Service name already exists");
            }
            var serviceCrud = new ServiceCrudFactory();
            serviceCrud.Create(service);
        }
        public void Update(Service service)
        {
            var serviceCrud = new ServiceCrudFactory();
            serviceCrud.Update(service);
        }
        public void Delete(int id)
        {
            var serviceCrud = new ServiceCrudFactory();
            serviceCrud.Delete(id);
        }
        public Service RetrieveById(int id)
        {
            var serviceCrud = new ServiceCrudFactory();
            return serviceCrud.RetrieveById(id);
        }
        public List<Service> RetrieveAll()
        {
            var serviceCrud = new ServiceCrudFactory();
            return serviceCrud.RetrieveAll();
        }

    }
}
