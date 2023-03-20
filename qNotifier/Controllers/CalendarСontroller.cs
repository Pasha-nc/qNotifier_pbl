using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using qNotifier.Models;
using qNotifier.ViewModels;

namespace qNotifier.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        UserManager<User> _userManager;

        public CalendarController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangeMonth(string selectedMonth, int offset)
        {
            string[] x = new string[2];

            int month = DateTime.UtcNow.Month, year = DateTime.UtcNow.Year;

            if (selectedMonth != null && selectedMonth != string.Empty)
            {
                x = selectedMonth.Split('.');

                bool correctInput = int.TryParse(x[0], out month);

                if (correctInput)
                {
                    correctInput = int.TryParse(x[1], out year);
                }

                if (!correctInput)
                {
                    year = DateTime.UtcNow.Year;
                    month = DateTime.UtcNow.Month;
                }
            }
            return Json(CalendarDataProvider.Provide(year, month, offset));
        }

        [HttpGet]
        public IActionResult GetCalendarData(string selectedMonth)
        {
            string[] x = new string[2];

            int month = DateTime.UtcNow.Month, year = DateTime.UtcNow.Year;

            if (selectedMonth != null && selectedMonth != string.Empty)
            {
                x = selectedMonth.Split('.');

                bool correctInput = int.TryParse(x[0], out month);

                if (correctInput)
                {
                    correctInput = int.TryParse(x[1], out year);
                }

                if (!correctInput)
                {
                    year = DateTime.UtcNow.Year;
                    month = DateTime.UtcNow.Month;
                }
            }

            return Json(CalendarDataProvider.Provide(year, month));
        }

        [Produces("application/json")]
        public async Task<IActionResult> GetDatesWithRecords(string selectedMonth)
        {
            var user = await _userManager.GetUserAsync(User);

            int month;

            string[] x = new string[2];

            IEnumerable<int?>? datesWithRecords = null;

            if (selectedMonth != null && selectedMonth != string.Empty)
            {

                x = selectedMonth.Split('.');

                bool correctInput = int.TryParse(x[0], out month);

                datesWithRecords = correctInput ? user?.Records?.Where(r => r.AppDateTime?.Date.Month == month)
                                .Select(r => r.AppDateTime?.Date.Day) : null;
            }

            return new JsonResult(datesWithRecords);
        }


    }
}
