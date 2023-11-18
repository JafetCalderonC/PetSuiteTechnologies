using DTOs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CoreApp.Utilities
{
    internal static class NormalizerUtility
    {
        // Method to normalize all string properties of a DTO
        public static void NormalizerDTO<TDO>(this TDO dto,params string[] ignore)
        {
            if (dto == null)
                return;

            var properties = dto.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (ignore.Contains(property.Name))
                    continue;


                if (property.PropertyType == typeof(string))
                {
                    var value = property.GetValue(dto)?.ToString();
                    value = NormalizerString(value);
                    property.SetValue(dto, value);
                }

                if (property.PropertyType == typeof(List<string>))
                {
                    var value = property.GetValue(dto) as List<string>;
                    for (int i = 0; i < value.Count; i++)
                    {
                        value[i] = NormalizerString(value[i]);
                    }
                }
            }
        }

        // Method to normalize a string
        public static string? NormalizerString(this string? value)
        {
            if (value != null)
            {
                // Remove white spaces, tabs, etc.
                value.Trim();
                value = Regex.Replace(value, @"\s+", " ");

                // all to lower case
                value = value.ToLower();
            }

            return value;
        }
    }
}