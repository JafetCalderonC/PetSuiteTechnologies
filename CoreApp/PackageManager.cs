﻿using DataAccess.CRUD;
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

        private bool Validate(Package package, out string? message)
        {
            message = null;
            return true;
        }

        public void Create(Package package)
        {
            // Default values

            package.PackageName = "Defecto";
            package.Description = "Defecto";
            package.RoomId = 1;
            package.PetBreedType = "Defecto";
            package.PetSize = "Defecto";
            package.PetAggressiveness = 1;
            package.Status = 1;
            package.Services = new List<int> { 1, 2 };
            package.CreatedDate = DateTime.UtcNow;
            package.ModifiedDate = DateTime.UtcNow;

            // Create user
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