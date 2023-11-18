using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;

namespace CoreApp.Utilities
{
    internal static class TwilioUtility
    {
        public static bool SendSms(string to, string from, string body)
        {
            try
            {
                var accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID", EnvironmentVariableTarget.User);
                var authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN", EnvironmentVariableTarget.User);
                if (string.IsNullOrWhiteSpace(accountSid) || string.IsNullOrWhiteSpace(authToken))
                {
                    throw new Exception("Error sending SMS");
                }

                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                    body: body,
                    from: new Twilio.Types.PhoneNumber(from),
                    to: new Twilio.Types.PhoneNumber(to)
                );

                return message.ErrorCode == null;
            }
            catch (ApiException ex)
            {
                return false;
            }
            catch (Exception)
            {
                throw new Exception("Error sending SMS");
            }
        }

        public static async Task<bool> SendEmailAsync(string from, string to, string subject, string body)
        {
            try
            {
                var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY", EnvironmentVariableTarget.User);
                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    throw new Exception("Error sending email");
                }
                             
                var client = new SendGridClient(apiKey);
                var msg = MailHelper.CreateSingleEmail(new EmailAddress(from), new EmailAddress(to), subject, "", body);
                var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
                
                // return response.StatusCode == System.Net.HttpStatusCode.Accepted;
                return response.StatusCode == System.Net.HttpStatusCode.Accepted;
            }
            catch (Exception)
            {
                throw new Exception("Error al enviar el correo electrónico");
            }
        }
    }
}
