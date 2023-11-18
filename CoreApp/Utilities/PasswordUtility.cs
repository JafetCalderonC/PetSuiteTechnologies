using CoreApp.Others;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Voice;

namespace CoreApp.Utilities
{
    internal static class PasswordUtility
    {
        public static string CreateSalt(int size)
        {
            byte[] buffer = new byte[size];
            RandomNumberGenerator.Fill(buffer);
            return Convert.ToBase64String(buffer);
        }

        public static string CreatePasswordHash(string password, string salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] saltedPassword = Encoding.UTF8.GetBytes(password + salt);
                byte[] hash = sha256.ComputeHash(saltedPassword);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        public static bool VerifyPassword(string password, string salt, string hash)
        {
            var passwordHash = CreatePasswordHash(password, salt);
            return passwordHash == hash;
        }

        private static string GetRandomCharacters(string possibleCharacters, int minLength, Random random)
        {
            int randomNumber = random.Next(0, 4);
            var result = new StringBuilder();

            for (int i = 0; i < minLength; i++)
            {
                result.Append(possibleCharacters[random.Next(possibleCharacters.Length)]);
            }

            return result.ToString();
        }

        public static string GeneratePassword(PasswordOptions options)
        {
            var passwordBuilder = new StringBuilder();
            var random = new Random();

            // Add random characters of each type to meet minimum criteria
            passwordBuilder.Append(GetRandomCharacters(options.LowerCase, options.MinLowerCase, random));
            passwordBuilder.Append(GetRandomCharacters(options.UpperCase, options.MinUpperCase, random));
            passwordBuilder.Append(GetRandomCharacters(options.Numbers, options.MinNumbers, random));
            passwordBuilder.Append(GetRandomCharacters(options.SpecialCharacters, options.MinSpecialCharacters, random));

            // if the password length is still less than the minimum, add random characters of any type
            for (int i = passwordBuilder.Length; i < options.MinPasswordLength; i++)
            {
                passwordBuilder.Append(options.LowerCase[random.Next(options.LowerCase.Length)]);
            }

            // Shuffle the constructed password to avoid predictable patterns
            var passwordArray = passwordBuilder.ToString().ToCharArray();
            passwordArray = passwordArray.OrderBy(x => random.Next()).ToArray();

            return new string(passwordArray);
        }

        public static bool ValidatePassword(string newPassword, PasswordOptions options, out string? validationMessage)
        {
            validationMessage = null;

            if (newPassword.Length < options.MinPasswordLength)
            {
                validationMessage = $"La contraseña debe tener al menos {options.MinPasswordLength} caracteres.";
                return false;
            }

            if (newPassword.Count(char.IsLower) < options.MinLowerCase)
            {
                validationMessage = $"La contraseña debe tener al menos {options.MinLowerCase} caracteres en minúscula.";
                return false;
            }

            if (newPassword.Count(char.IsUpper) < options.MinUpperCase)
            {
                validationMessage = $"La contraseña debe tener al menos {options.MinUpperCase} caracteres en mayúscula.";
                return false;
            }

            if (newPassword.Count(char.IsDigit) < options.MinNumbers)
            {
                validationMessage = $"La contraseña debe tener al menos {options.MinNumbers} números.";
                return false;
            }

            if (newPassword.Count(c => options.SpecialCharacters.Contains(c)) < options.MinSpecialCharacters)
            {
                validationMessage = $"La contraseña debe tener al menos {options.MinSpecialCharacters} caracteres especiales.";
                return false;
            }

            return true;
        }
    }
}