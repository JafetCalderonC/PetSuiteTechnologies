using CoreApp.Others;
using CoreApp.Utilities;
using DataAccess.CRUD;
using DTOs;
using DTOs.User;
using Microsoft.SqlServer.Server;
using SendGrid.Helpers.Mail.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreApp
{
    public class UserManager
    {
        private readonly UserCrudFactory _crud;
        private readonly PasswordOptions _passwordOptions;
        private readonly int saltSize = 16;

        public UserManager(PasswordOptions passwordOptions)
        {
            _crud = new UserCrudFactory();
            _passwordOptions = passwordOptions;
        }

        private void EnsureGeneralvalidation(User user, bool isNewUser)
        {
            if (user == null)
            {
                throw new ValidationException("El usuario no puede ser nulo");
            }

            // Validate first name
            if (string.IsNullOrWhiteSpace(user.FirstName) || user.FirstName.Length < 4)
            {
                throw new ValidationException("El nombre no puede ser menor a 4 caracteres");
            }

            // Validate last name
            if (string.IsNullOrWhiteSpace(user.LastName) || user.LastName.Length < 4)
            {
                throw new ValidationException("El apellido no puede ser menor a 4 caracteres");
            }

            // Validate email
            if (string.IsNullOrWhiteSpace(user.Email) || !Regex.IsMatch(user.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                throw new ValidationException("El correo electrónico no es válido");
            }
            if (user.Email.Count(x => x == '@') != 1)
            {
                throw new ValidationException("El correo electrónico no es válido");
            }

            // Validate identification type
            if (string.IsNullOrWhiteSpace(user.IdentificationType) || (user.IdentificationType == "passport" || user.IdentificationType == "national-id" || user.IdentificationType == "dimex") == false)
            {
                throw new ValidationException("La identificación no es válida");
            }

            // Validate profile photo base64 (png, jpg, jpeg, gif)
            if (string.IsNullOrWhiteSpace(user.ProfilePhoto) || ((
                user.ProfilePhoto.Contains("data:image/;base64")      ||
                user.ProfilePhoto.Contains("data:image/png;base64,")  ||
                user.ProfilePhoto.Contains("data:image/jpg;base64,")  ||
                user.ProfilePhoto.Contains("data:image/jpeg;base64,") ||
                user.ProfilePhoto.Contains("data:image/gif;base64,")) == false))
            {
                throw new ValidationException("La foto de perfil no es válida solo se aceptan archivos png, jpg, jpeg y gif");
            }

            // Validate address latitude
            if (user.AddressLatitude < -90 || user.AddressLatitude > 90)
            {
                throw new ValidationException("La latitud y longitud no son válidas");
            }

            #region Identification type validation
            if (string.IsNullOrWhiteSpace(user.IdentificationValue))
            {
                throw new ValidationException("La identificación no puede ser nula");
            }

            if (user.IdentificationType == "PASSPORT" && !Regex.IsMatch(user.IdentificationValue, @"^[A-Z]{2}[0-9]{7}$"))
            {
                throw new ValidationException("El número de pasaporte no es válido");
            }

            if (user.IdentificationType == "NATIONAL-ID" && !Regex.IsMatch(user.IdentificationValue, @"^[0-9]{9}$"))
            {
                throw new ValidationException("La cédula de identidad no es válida");
            }

            if (user.IdentificationType == "DIMEX" && !Regex.IsMatch(user.IdentificationValue, @"^[0-9]{11}$"))
            {
                throw new ValidationException("El DIMEX no es válido");
            }
            #endregion

            // Check if email is already registered
            if (isNewUser)
            {
                if (_crud.RetrieveByEmail(user.Email) != null)
                {
                    throw new ValidationException("Este correo electrónico ya está registrado");
                }
            }
            // Check if email is already registered in another user
            else
            {
                var userFromDb = _crud.RetrieveById(user.Id) ?? throw new ValidationException("El usuario no existe");
                if (userFromDb != null && userFromDb.Email != user.Email)
                {
                    if (_crud.RetrieveByEmail(user.Email) != null)
                    {
                        throw new ValidationException("Este correo electrónico ya está registrado");
                    }
                }
            }

            #region validation phone numbers 
            List<string> validPhoneNumbers = new List<string>();

            // Check if phone number already exists in another user
            foreach (string phoneNumber in user.PhoneNumbers)
            {
                // Validate phone number
                if (Regex.IsMatch(phoneNumber, @"^[0-9]{8}$"))
                {
                    // Check if phone number is duplicated
                    if (validPhoneNumbers.Contains("+506" + phoneNumber))
                        continue;

                    // Check if phone number already exists
                    if (isNewUser)
                    {
                        if (_crud.RetrieveByPhoneNumber("+506" + phoneNumber) != null)
                        {
                            throw new ValidationException($"El número de teléfono {phoneNumber} ya está registrado");
                        }
                    }
                    // Check if phone number already exists in another user
                    else
                    {
                        var userPhone = _crud.RetrieveByPhoneNumber("+506" + phoneNumber);
                        if (userPhone != null && userPhone.Id != user.Id)
                        {
                            throw new ValidationException($"El número de teléfono {phoneNumber} ya está registrado");
                        }
                    }

                    validPhoneNumbers.Add("+506" + phoneNumber);
                }
                else
                {
                    throw new ValidationException($"El número de teléfono {phoneNumber} no es válido");
                }
            }

            // Check if there is at least one phone number
            if (validPhoneNumbers.Count == 0)
            {
                throw new ValidationException("Se debe ingresar al menos un número de teléfono");
            }

            user.PhoneNumbers = validPhoneNumbers;
            #endregion

            if (user.Role != "admin" && user.Role != "gestor" && user.Role != "client")
            {
                throw new ValidationException("El rol no es válido");
            }

            if (user.Status != 1 && user.Status != 2)
            {
                throw new ValidationException("El estado no es válido");
            }
        }

        // Method to register a new user as Admin or Gestor
        public void Create(User user)
        {
            user.NormalizerDTO("ProfilePhoto");

            // General validation
            EnsureGeneralvalidation(user, true);

            // Generate password
            string password = PasswordUtility.GeneratePassword(_passwordOptions);
            string salt = PasswordUtility.CreateSalt(saltSize);
            string passwordHash = PasswordUtility.CreatePasswordHash(password, salt);

            // Save profile photo
            var publicId = CloudinaryUtility.UploadImage("PetSuite Technologies/Profile photos", user.ProfilePhoto);

            // Send SMS
            var responseSMS = TwilioUtility.SendSms(user.PhoneNumbers.First(), "+12054311884", $"Bienvenido a PetSuite {user.FirstName} {user.LastName}, su contraseña es: {password}");
            if (!responseSMS)
            {
                throw new ValidationException($"Lo sentimos, para terminar de crear su cuenta se itentó enviar un mensaje de texto a {user.PhoneNumbers.First()}, pero no se pudo enviar, por favor verifique que el número de teléfono sea correcto");
            }

            // Send email
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string htmlContent = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/welcome-email.html"), Encoding.UTF8);
            htmlContent = htmlContent.Replace("{{first-name}}", textInfo.ToTitleCase(user.FirstName));
            htmlContent = htmlContent.Replace("{{last-name}}", textInfo.ToTitleCase(user.LastName));
            htmlContent = htmlContent.Replace("{{link-login}}", "https://petsuitecr.azurewebsites.net/");
            htmlContent = htmlContent.Replace("{{support-email}}", "support@petsuitetech.com");
            htmlContent = htmlContent.Replace("{{support-phone}}", "+506 8888-8888");
            var responseEmal = TwilioUtility.SendEmailAsync("jmoscosoa@ucenfotec.ac.cr", user.Email, "Bienvenido a PetSuite", htmlContent);
            responseEmal.Wait(); // Wait for the email to be sent
            if (responseEmal.Result == false)
            {
                throw new ValidationException($"Lo sentimos, no se pudo enviar el correo electrónico a {user.Email}, por favor verifique que el correo electrónico sea correcto");
            }


            // Save user
            user.PasswordHash = passwordHash;
            user.PasswordSalt = salt;
            user.IsPasswordRequiredChange = true;
            user.CreatedDate = DateTime.UtcNow;
            user.ModifiedDate = DateTime.UtcNow;
            user.ThemePreference = "light";
            user.CloudinaryPublicId = publicId;

            _crud.Create(user);
        }

        public void Update(User user)
        {
            user.NormalizerDTO("ProfilePhoto");

            // General validation
            EnsureGeneralvalidation(user, false);

            // Get user from database
            var userFromDb = _crud.RetrieveById(user.Id) ?? throw new ValidationException("El usuario no existe");
        
            // Upload new image
            userFromDb.CloudinaryPublicId =  CloudinaryUtility.UploadImage("PetSuite Technologies/Profile photos", user.ProfilePhoto);

            // Update data
            userFromDb.FirstName = user.FirstName;
            userFromDb.LastName = user.LastName;
            userFromDb.Email = user.Email;
            userFromDb.PhoneNumbers = user.PhoneNumbers;
            userFromDb.ModifiedDate = DateTime.UtcNow;
            userFromDb.IdentificationType = user.IdentificationType;
            userFromDb.IdentificationValue = user.IdentificationValue;
            userFromDb.Role = user.Role;
            userFromDb.Status = user.Status;
            userFromDb.AddressLatitude = user.AddressLatitude;
            userFromDb.AddressLongitude = user.AddressLongitude;
            _crud.Update(userFromDb);
        }

        public void Delete(int id)
        {
            var userFromDb = _crud.RetrieveById(id) ?? throw new ValidationException("El usuario no existe");
            // Delete profile photo
            CloudinaryUtility.DeleteImage(userFromDb.CloudinaryPublicId);
            _crud.Delete(id);
        }

        public User? RetrieveById(int id)
        {
            var userFromDb = _crud.RetrieveById(id) ?? throw new ValidationException("El usuario no existe");

            // get profile photo
            userFromDb.ProfilePhoto = CloudinaryUtility.GetImage(userFromDb.CloudinaryPublicId);
            // get phone numbers
            userFromDb.PhoneNumbers = _crud.RetrieveAllPhoneNumbersByUserId(userFromDb.Id);

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

            // Remove sensitive data
            userFromDb.PasswordHash = null;
            userFromDb.PasswordSalt = null;
            userFromDb.CloudinaryPublicId = null;
            userFromDb.FirstName = textInfo.ToTitleCase(userFromDb.FirstName);
            userFromDb.LastName = textInfo.ToTitleCase(userFromDb.LastName);

            return userFromDb;
        }

        public List<User> RetrieveAll()
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            List<User> users = new List<User>();

             foreach (var user in _crud.RetrieveAll())
            {
                // get profile photo
                user.ProfilePhoto = CloudinaryUtility.GetImage(user.CloudinaryPublicId);
                // get phone numbers
                user.PhoneNumbers = _crud.RetrieveAllPhoneNumbersByUserId(user.Id);

                // Remove sensitive data
                user.PasswordHash = null;
                user.PasswordSalt = null;
                user.CloudinaryPublicId = null;
                user.FirstName = textInfo.ToTitleCase(user.FirstName);
                user.LastName = textInfo.ToTitleCase(user.LastName);

                users.Add(user);
            }

            return users;
        }

        public int Login(Login login)
        {
            login.NormalizerDTO("Password");

            var userFromDb = _crud.RetrieveByEmail(login.Email) ?? throw new ValidationException("Correo electrónico o contraseña incorrectos");

            if (userFromDb.Status == 2)
            {
                throw new ValidationException("Su cuenta ha sido deshabilitada");
            }

            // Validate password
            if (!PasswordUtility.VerifyPassword(login.Password, userFromDb.PasswordSalt, userFromDb.PasswordHash))
            {
                throw new ValidationException("Correo electrónico o contraseña incorrectos");
            }

            userFromDb.ModifiedDate = DateTime.UtcNow;
            _crud.Update(userFromDb);

            return userFromDb.Id;
        }

        public void ForgotPassword(ForgotPassword forgot)
        {
            try
            {
                forgot.NormalizerDTO();

                var userFromDb = _crud.RetrieveByEmail(forgot.Email);
                if (userFromDb != null)
                {
                    // Generate password
                    string password = PasswordUtility.GeneratePassword(_passwordOptions);
                    string salt = PasswordUtility.CreateSalt(saltSize);
                    string passwordHash = PasswordUtility.CreatePasswordHash(password, salt);



                    TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

                    string htmlContent = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/forgot-password-email.html"), Encoding.UTF8);
                    htmlContent = htmlContent.Replace("{{first-name}}", textInfo.ToTitleCase(userFromDb.FirstName));
                    htmlContent = htmlContent.Replace("{{last-name}}", textInfo.ToTitleCase(userFromDb.LastName));
                    htmlContent = htmlContent.Replace("{{link-login}}", "https://petsuitecr.azurewebsites.net/");
                    htmlContent = htmlContent.Replace("{{support-email}}", "support@petsuitetech.com");
                    htmlContent = htmlContent.Replace("{{support-phone}}", "+506 8888-8888");
                    htmlContent = htmlContent.Replace("{{password}}", password);

                    // Send email
                    _ = TwilioUtility.SendEmailAsync("jmoscosoa@ucenfotec.ac.cr", userFromDb.Email, "Recuperar contraseña", htmlContent);

                    // Update password
                    userFromDb.PasswordHash = passwordHash;
                    userFromDb.PasswordSalt = salt;
                    userFromDb.ModifiedDate = DateTime.UtcNow;
                    userFromDb.IsPasswordRequiredChange = true;
                    _crud.Update(userFromDb);
                }
            }
            catch (Exception)
            {
                throw new ValidationException("Lo sentimos, algo salió mal. Inténtalo de nuevo más tarde o comunícate soporte técnico.");
            }

        }

        public void ChangePassword(ChangePassword change)
        {
            change.NormalizerDTO("CurrentPassword", "NewPassword", "ConfirmNewPassword");

            var userFromDb = _crud.RetrieveByEmail(change.Email) ?? throw new ValidationException("El usuario no existe");

            // Validate password
            if (!PasswordUtility.VerifyPassword(change.CurrentPassword, userFromDb.PasswordSalt, userFromDb.PasswordHash))
            {
                throw new ValidationException("La contraseña actual es incorrecta");
            }

            if (change.CurrentPassword == change.NewPassword)
            {
                throw new ValidationException("La contraseña nueva no puede ser igual a la actual");
            }

            if (change.NewPassword != change.ConfirmNewPassword)
            {
                throw new ValidationException("La contraseña nueva no coincide con la confirmación");
            }

            if (change.CurrentPassword == change.ConfirmNewPassword)
            {
                throw new ValidationException("La contraseña nueva no puede ser igual a la actual");
            }

            // Validate new password
            if (!PasswordUtility.ValidatePassword(change.NewPassword, _passwordOptions, out string errorMessage))
            {
                throw new ValidationException(errorMessage);
            }

            // Update password
            string salt = PasswordUtility.CreateSalt(saltSize);
            string passwordHash = PasswordUtility.CreatePasswordHash(change.NewPassword, salt);

            userFromDb.PasswordHash = passwordHash;
            userFromDb.PasswordSalt = salt;
            userFromDb.ModifiedDate = DateTime.UtcNow;
            userFromDb.IsPasswordRequiredChange = false;

            _crud.Update(userFromDb);
        }
    }
}