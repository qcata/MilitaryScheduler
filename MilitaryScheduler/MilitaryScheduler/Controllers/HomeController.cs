using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MilitaryScheduler.Data;
using MilitaryScheduler.Models;
using MilitaryScheduler.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MilitaryScheduler.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult LoadEventPage(string startDate)
        {
            return Json(new { result = "Redirect", url = Url.Action("CreateEventForAdmin", "Home") + "?startDate=" + startDate });
        }

        public IActionResult CreateEventForAdmin(string startDate)
        {
            var model = new EventViewModel()
            {
                ApplicationUsers = new List<SelectListItem>()
            };

            var date = startDate.Substring(0, startDate.IndexOf('T'));
            model.Start = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            var appUsers = _context.Users.Where(u => !u.IsSystemAdmin).ToList();
            foreach (var user in appUsers)
            {
                model.ApplicationUsers.Add(new SelectListItem()
                {
                    Value = user.Id,
                    Text = user.Email
                });
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateEventForAdmin(EventViewModel model)
        {
            
            return View(model);
        }
    }
}
