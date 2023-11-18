using DTOs.User;
using System.ComponentModel.DataAnnotations;

namespace CoreApp
{
    public abstract class BaseManager
    {
        protected User _userAuth;

        /// <summary>
        /// Method to validate the user authorization
        /// </summary>
        /// <exception cref="ValidationException"></exception>
        protected void ValidateUserAuth()
        {
            if (_userAuth == null)
                throw new ValidationException("Usuario no autorizado");

            if (_userAuth.Role == "admin" || _userAuth.Role == "gestor" || _userAuth.Role == "client")
                return; // The user is authorized

            throw new ValidationException("Usuario no autorizado");
        }

        /// <summary>
        /// Method to validate the user authorization by role
        /// </summary>
        /// <param name="roles"></param>
        /// <exception cref="ValidationException"></exception>
        protected void ValidateUserAuth(params string[] roles)
        {
            if (_userAuth == null)
                throw new ValidationException("Usuario no autorizado");


            foreach (var role in roles)
            {
                if (_userAuth.Role == role)
                    return; // The user is authorized
            }

            throw new ValidationException("Usuario no autorizado");
        }

        /// <summary>
        /// Method ensure user cannot access or modify data not authorized
        /// </summary>
        /// <param name="user"></param>
        /// <exception cref="ValidationException"></exception>
        protected void EnsureUserCannotAccessData(User? user)
        {
            if (CanAccessUserData(user) == false)
            {
                throw new ValidationException("Usuario no autorizado");
            }
        }

        /// <summary>
        /// Method access to user data
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected bool CanAccessUserData(User? user)
        {
            if (user == null)
                return false;

            // The user can access their own data
            if (_userAuth.Id == user.Id)
                return true;

            // Gestor can access client data
            if (_userAuth.Role == "gestor" && user.Role == "client")
                return true;

            // Gestor not can access other gestor data
            if (_userAuth.Role == "gestor" && user.Role == "admin")
                return false;

            // Gestor not can access admin data
            if (_userAuth.Role == "gestor" && user.Role == "admin")
                return false;

            // Admin can access all data
            if (_userAuth.Role == "admin")
                return true;

            // User can not access the data
            return false;
        }
    }
}