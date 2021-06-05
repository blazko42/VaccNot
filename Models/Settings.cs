using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
namespace VaccNot.Models
{
    public class Settings
    {
        public string URL { get; set; } = @"https:/<platform_url>/scheduling/api/centres?page=0&size=20&sort=,";
        public string CountyId { get; set; } = "40";
        public string IdentificationCode { get; set; } = "<SSN>";
        public string LocalityID { get; set; } = "";
        public string MasterPersonnelCategoryId { get; set; } = "-4";
        public string Name { get; set; } = "";
        public string PersonnelCategoryID { get; set; } = "32";
        public string RecipientID { get; set; } = "<recipientId>";
        public string Cookie { get; set; } = "";

        public void GetCookie()
        {
            try
            {

                EdgeOptions options = new EdgeOptions();

                options.UseChromium = true;
                options.AddArgument("headless");
                options.AddArgument("disable-gpu");

                EdgeDriver driver = new EdgeDriver(options);

                driver.Navigate().GoToUrl("https://<platform_url>/auth/login");

                driver.FindElement(By.Name("username")).SendKeys("<email>");
                driver.FindElement(By.Name("password")).SendKeys("<password>");
                driver.FindElement(By.XPath("/html/body/app/vertical-layout-1/div/div/div[1]/div/content/login/div/div/div/mat-tab-group/div/mat-tab-body[1]/div/app-authenticate-with-email/form/button")).SendKeys((Keys.Enter));

                Thread.Sleep(6000);

                foreach (var cookie in driver.Manage().Cookies.AllCookies)
                {
                    if (cookie.Name == "SESSION")
                        Cookie = cookie.Name + "=" + cookie.Value;
                }

                driver.Quit();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<string> GetListOfSlotsJSON()
        {
            try
            {
                object input = new
                {
                    countyID = CountyId,
                    identificationCode = IdentificationCode,
                    localityID = LocalityID,
                    masterPersonnelCategoryID = MasterPersonnelCategoryId,
                    name = Name,
                    personnelCategoryID = PersonnelCategoryID,
                    recipientID = RecipientID
                };

                using HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Add("cookie", Cookie);
                HttpResponseMessage response = await client.PostAsync(URL, new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json"));

                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static void VaccineSlotsWatch(Settings settings)
        {
            System.Timers.Timer timer = new System.Timers.Timer(TimeSpan.FromMinutes(30).TotalMilliseconds);
            timer.AutoReset = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler((sender, e) => GetSlots(settings, e));
            timer.Start();

        }

        public static async void GetSlots(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            try
            {

                ((Settings) sender).GetCookie();

                List<VaccineSlot> vaccineSlots = JsonResponseClass.ParseJSONVaccineSlotsResponse(await ((Settings) sender).GetListOfSlotsJSON());

                Notification notification = new Notification();

                notification.ComposeMessage(vaccineSlots);

                if (!string.IsNullOrWhiteSpace(notification.NotificationMessage))
                    Notification.SendEmailNotification(notification);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}