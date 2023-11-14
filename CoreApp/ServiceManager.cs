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
            message = null;
            return true;
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
