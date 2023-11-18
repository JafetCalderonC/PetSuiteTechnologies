using DataAccess.CRUD;
using DataAccess.DAOs;
using DTOs.Service;
using DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp
{
    public class ServiceManager
    {
        private ServiceCrudFactory _crud;

        public ServiceManager()
        {
            _crud = new ServiceCrudFactory();
        }

        private bool Validate(Service user, out string? message)
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
        public void Delete(BaseDTO dto)
        {
            var serviceCrud = new ServiceCrudFactory();
            serviceCrud.Delete(dto);
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

        public void Create(Service user)
        {
            // Default values
            //user.Role = "client";
            //user.Status = 1;
            //user.IsOtpVerified = false;
            //user.CreatedDate = DateTime.UtcNow;
            //user.ModifiedDate = DateTime.UtcNow;
            //user.ThemePreference = "light";

            // Create user
            _crud.Create(user);
        }

        public void Update(Service user)
        {
            _crud.Update(user);
        }
    }
}
