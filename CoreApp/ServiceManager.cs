using CoreApp.Utilities;
using DataAccess.CRUD;
using DTOs;
using DTOs.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        private void EnsureGeneralvalidation(Service service, bool isNewService)
        {
            if (service == null)
            {
                throw new Exception("El servicio es nulo");
            }

            if (string.IsNullOrEmpty(service.ServiceName) || string.IsNullOrWhiteSpace(service.ServiceName))
            {
                throw new Exception("El nombre del servicio es requerido");
            }

            if (string.IsNullOrEmpty(service.ServiceDescription) || string.IsNullOrWhiteSpace(service.ServiceDescription))
            {
                throw new Exception("La descripción del servicio es requerida");
            }

            if (service.ServiceCost <= 0)
            {
                throw new Exception("El precio tiene que ser mayor a 0");
            }

            if ((service.ServiceStatus == 1 || service.ServiceStatus == 2) == false)
            {
                throw new Exception("El estado del servicio es inválido");
            }

            if (_crud.RetrieveAll().Any(x => x.ServiceName == service.ServiceName && x.Id != service.Id))
            {
                throw new ValidationException("Servicio ya existe con el mismo nombre");
            }
        }

        public void Create(Service service)
        {
            service.NormalizerDTO();
            EnsureGeneralvalidation(service, true);

            service.ServiceCreatedDate = DateTime.Now;
            service.ServiceModifiedDate = DateTime.Now;
            _crud.Create(service);
        }
        public void Update(Service service)
        {
            service.NormalizerDTO();
            EnsureGeneralvalidation(service, false);

            // get service by id
            var currentService = _crud.RetrieveById(service.Id);
            if (currentService == null)
            {
                throw new Exception("El servicio no existe");
            }

            currentService.ServiceName = service.ServiceName;
            currentService.ServiceDescription = service.ServiceDescription;
            currentService.ServiceStatus = service.ServiceStatus;
            currentService.ServiceCost = service.ServiceCost;
            currentService.ServiceModifiedDate = DateTime.Now;
            _crud.Update(currentService);
        }
        public void Delete(int id)
        {
            // get service by id
            var currentService = _crud.RetrieveById(id);
            if (currentService == null)
            {
                throw new Exception("El servicio no existe");
            }

            _crud.Delete(id);
        }
        public Service RetrieveById(int id)
        {
            // get service by id
            var currentService = _crud.RetrieveById(id);
            if (currentService == null)
            {
                throw new Exception("El servicio no existe");
            }

            // Capitalize first letter
            currentService.ServiceName = char.ToUpper(currentService.ServiceName[0]) + currentService.ServiceName.Substring(1);
            currentService.ServiceDescription = char.ToUpper(currentService.ServiceDescription[0]) + currentService.ServiceDescription.Substring(1);

            return currentService;
        }
        public List<Service> RetrieveAll()
        {
            var services = new List<Service>();

            foreach (var service in _crud.RetrieveAll())
            {
                // Capitalize first letter
                service.ServiceName = char.ToUpper(service.ServiceName[0]) + service.ServiceName.Substring(1);
                service.ServiceDescription = char.ToUpper(service.ServiceDescription[0]) + service.ServiceDescription.Substring(1);

                services.Add(service);
            }
            return services;
        }
    }
}