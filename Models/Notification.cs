using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace VaccNot.Models
{
    public class Notification
    {
        public static string Server { get; set; } = "smtp.gmail.com";
        public static string From { get; set; } = "vaccnotif@gmail.com";
        public static string Password { get; set; } = "<emailPassword>";
        public static string To { get; set; } = "<toEmail>";
        public static string CC { get; set; } = "";
        public static int Port { get; set; } = 587;
        public static bool EnableSSL { get; set; } = true;
        public static bool IsBodyHtml { get; set; } = true;
        public static bool UseDefaultCredentials { get; set; } = false;
        public static string Subject { get; set; } = "Locuri de vaccinare disponibile!";
        public static string DefaultMessage { get; set; } = "<div class=\"row mb-4\">\r\n<div class=\"col\">\r\n<p>Vaccin disponibil: <span style=\"color: red;\">$VACCINE_NAME$ ($VACCINE_ID$)</span>;\r\n$AVAILABLE_SLOTS$ locuri disponibile la centrul\r\n$SLOT_NAME$ in $SLOT_COUNTY$, $SLOT_LOCALITY$.</p>\r\n</div>\r\n</div>";

        public string NotificationMessage { get; set; } = "";

        public static void SendEmailNotification(Notification notification)
        {
            try
            {
                MailMessage message = new MailMessage(From, To);

                message.Subject = Subject;
                message.Body = notification.NotificationMessage;
                message.IsBodyHtml = IsBodyHtml;

                if (!string.IsNullOrWhiteSpace(CC))
                    message.CC.Add(CC);

                SmtpClient client = new SmtpClient();
                client.Host = Server;
                client.Port = Port;
                client.EnableSsl = EnableSSL;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = UseDefaultCredentials;
                client.Credentials = new NetworkCredential(From, Password);

                client.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ComposeMessage(List<VaccineSlot> slots)
        {
            try
            {
                foreach (VaccineSlot slot in slots)
                {
                    if (slot.AvailableSlots > 0)
                    {
                        NotificationMessage += DefaultMessage.Replace("$VACCINE_NAME$", ((Booster) slot.BoosterId).ToString())
                            .Replace("$VACCINE_ID$", slot.BoosterId.ToString())
                            .Replace("$AVAILABLE_SLOTS$", slot.AvailableSlots.ToString())
                            .Replace("$SLOT_NAME$", slot.Name)
                            .Replace("$SLOT_COUNTY$", slot.CountyName)
                            .Replace("$SLOT_LOCALITY$", slot.LocalityName);
                    }
                }

                if (!string.IsNullOrWhiteSpace(NotificationMessage))
                    NotificationMessage += "<div class=\"row mb-4\">\r\n<div class=\"col\">\r\n<h2><a target=\"_blank\" href=\"https://programare.vaccinare-covid.gov.ro/auth/login\">Quick Login</a></h2>\r\n</div>\r\n</div>";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}