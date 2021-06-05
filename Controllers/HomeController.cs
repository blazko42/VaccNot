using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VaccNot.Models;

namespace VaccNot.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(new Settings());
        }

        public IActionResult VaccineSlotsWatch(Settings settings)
        {
            settings.GetCookie();

            return View(settings);
        }

        public async Task<IActionResult> VaccineSlotsCheck(Settings settings)
        {
            settings.GetCookie();

            List<VaccineSlot> vaccineSlots = JsonResponseClass.ParseJSONVaccineSlotsResponse(await settings.GetListOfSlotsJSON());

            Notification notification = new Notification();

            notification.ComposeMessage(vaccineSlots);

            if (!string.IsNullOrWhiteSpace(notification.NotificationMessage))
                Notification.SendEmailNotification(notification);

            return View(vaccineSlots);
        }

    }
}