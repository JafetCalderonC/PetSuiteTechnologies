using DataAccess.CRUD;
using DataAccess.DAOs;
using DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp
{
    public class UserManager
    {
        private UserCrudFactory _crud;

        public UserManager()
        {
            _crud = new UserCrudFactory();
        }

        private bool Validate(User user, out string? message)
        {
            message = null;
            return true;
        }

        public void Create(User user)
        {
            // Default values
            user.Role = "client";
            user.Status = 1;
            user.IsOtpVerified = false;
            user.CreatedDate = DateTime.UtcNow;
            user.ModifiedDate = DateTime.UtcNow;
            user.ThemePreference = "light";

            // Create user
            _crud.Create(user);
        }

        public void Update(User user)
        {
            _crud.Update(user);
        }
    }
}
