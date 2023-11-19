using DataAccess.CRUD;
using DTOs;
using DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp
{
    public class PackageManager
    {
        private PackageCrudFactory _crud;

        public PackageManager()
        {
            _crud = new PackageCrudFactory();
        }

        private void EnsureGeneralvalidation(Package package, bool isNewService)
        {
            if (package == null)
            {
                throw new Exception("El servicio es nulo");
            }

            if (string.IsNullOrEmpty(package.PackageName) || string.IsNullOrWhiteSpace(package.PackageName))
            {
                throw new Exception("El nombre del servicio es requerido");
            }

            if (string.IsNullOrEmpty(package.Description) || string.IsNullOrWhiteSpace(package.Description))
            {
                throw new Exception("La descripción del servicio es requerida");
            }

            if (package.RoomId <= 0)
            {
                throw new Exception("la habitacion tiene que ser mayor a 0");
            }

            if ((package.Status == 1 || package.Status == 2) == false)
            {
                throw new Exception("El estado del servicio es inválido");
            }


            if (isNewService == true)
            {
                // Validate if the service already exists by name
                var currentService = _crud.RetrieveAll();
                if (currentService != null)
                {
                    foreach (var item in currentService)
                    {
                        if (item.PackageName == package.PackageName)
                            throw new Exception("El servicio ya existe");
                    }
                }
            }

        }

    public void Create(Package package)
        {
            EnsureGeneralvalidation(package, true);

            _crud.Create(package);
        }

        public void Update(Package package)
        {
            _crud.Update(package);
        }

        public List<Package> RetrieveAll()
        {
            var uc = new PackageCrudFactory();

            return uc.RetrieveAll();
        }

    }
}