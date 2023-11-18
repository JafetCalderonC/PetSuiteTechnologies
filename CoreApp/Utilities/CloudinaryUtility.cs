using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;
using Twilio;
using Microsoft.Identity.Client;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace CoreApp.Utilities
{
    internal static class CloudinaryUtility
    {
        public static string GetImage(string publicId)
        {
            try
            {
                var cloudinary_url = Environment.GetEnvironmentVariable("CLOUDINARY_URL", EnvironmentVariableTarget.User);
                if (string.IsNullOrWhiteSpace(cloudinary_url))
                {
                    throw new Exception("Error getting image");
                }

                Cloudinary cloudinary = new Cloudinary(cloudinary_url);
                cloudinary.Api.Secure = true;

                // Get image from cloudinary and convert to base64
                var url = cloudinary.Api.UrlImgUp.Secure().Transform(new Transformation().Width(100).Height(100).Crop("thumb")).BuildUrl(publicId);

                // Get image format
                string format = url.Contains("png") ? "png" :
                                url.Contains("jpg") ? "jpg" :
                                url.Contains("jpeg") ? "jpeg" :
                                url.Contains("gif") ? "gif" : "";

                // Download image
                HttpClient client = new HttpClient();
                byte[] imageBytes = client.GetByteArrayAsync(url).Result;

                // Convertir a Base64 png
                return "data:image/" + format + ";base64," + Convert.ToBase64String(imageBytes);
            }
            catch (Exception)
            {
                throw new Exception("Error getting image");
            }
        }

        public static string UploadImage(string folder, string imageBase64)
        {
            try
            {
                var cloudinary_name = Environment.GetEnvironmentVariable("CLOUDINARY_NAME", EnvironmentVariableTarget.User);
                var cloudinary_api_key = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY", EnvironmentVariableTarget.User);
                var cloudinary_api_secret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET", EnvironmentVariableTarget.User);
                if (string.IsNullOrWhiteSpace(cloudinary_api_secret) || string.IsNullOrWhiteSpace(cloudinary_api_key) || string.IsNullOrWhiteSpace(cloudinary_name))
                {
                    throw new Exception("Error uploading image");
                }

                Cloudinary cloudinary = new Cloudinary(new Account(cloudinary_name, cloudinary_api_key, cloudinary_api_secret));
                cloudinary.Api.Secure = true;

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imageBase64),
                    Folder = folder,
                };

                var uploadResult = cloudinary.Upload(uploadParams);

                // if the image is not uploaded
                if (uploadResult.Error != null)
                {
                    throw new Exception("Error uploading image");
                }

                // Get public ID
                var publicId = uploadResult.JsonObj["public_id"]?.ToString();
                if (string.IsNullOrWhiteSpace(publicId))
                {
                    throw new Exception("Error uploading image");
                }

                return publicId;
            }
            catch (Exception)
            {
                throw new Exception("Error uploading image");
            }
        }

        public static void DeleteImage(string publicId)
        {
            try
            {
                var cloudinary_url = Environment.GetEnvironmentVariable("CLOUDINARY_URL", EnvironmentVariableTarget.User);
                if (string.IsNullOrWhiteSpace(cloudinary_url))
                {
                    throw new Exception("Error deleting image");
                }

                Cloudinary cloudinary = new Cloudinary(cloudinary_url);
                cloudinary.Api.Secure = true;

                var deleteParams = new DeletionParams(publicId);
                var result = cloudinary.Destroy(deleteParams);

                if (result.Error != null)
                {
                    throw new Exception("Error deleting image");
                }
            }
            catch (Exception)
            {
                throw new Exception("Error deleting image");
            }
        }


        public static void updateImage(string publicId, string imageBase64)
        {
            try
            {
                var cloudinary_name = Environment.GetEnvironmentVariable("CLOUDINARY_NAME", EnvironmentVariableTarget.User);
                var cloudinary_api_key = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY", EnvironmentVariableTarget.User);
                var cloudinary_api_secret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET", EnvironmentVariableTarget.User);
                if (string.IsNullOrWhiteSpace(cloudinary_api_secret) || string.IsNullOrWhiteSpace(cloudinary_api_key) || string.IsNullOrWhiteSpace(cloudinary_name))
                {
                    throw new Exception("Error uploading image");
                }

                Cloudinary cloudinary = new Cloudinary(new Account(cloudinary_name, cloudinary_api_key, cloudinary_api_secret));
                cloudinary.Api.Secure = true;

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imageBase64),
                    PublicId = publicId,
                };


                var uploadResult = cloudinary.Upload(uploadParams);

                // if the image is not uploaded
                if (uploadResult.Error != null)
                {
                    throw new Exception("Error uploading image");
                }
            }
            catch (Exception)
            {
                throw new Exception("Error uploading image");
            }
        }
    }
}
