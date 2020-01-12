using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MilitaryScheduler.Data;
using MilitaryScheduler.Models;
using MilitaryScheduler.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MilitaryScheduler.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RequestsController(ApplicationDbContext context,
                                  UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (_userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)).Result.IsSystemAdmin)
            {
                return View(PrepareModel());
            }
            else
            {
                return View("MyRequests", PrepareModel());
            }
        }

        public IActionResult MyRequests()
        {
            return View(PrepareModel());
        }



        public IActionResult ApproveEventRequest(string requestId)
        {
            if (int.TryParse(requestId, out int rid))
            {
                var request = _context.Requests.First(r => r.Id == rid);
                if (int.TryParse(request.EventId, out int evId))
                {
                    var calendarEvent = _context.Events.FirstOrDefault(e => e.Id == evId);
                    if (calendarEvent != null)
                    {
                        calendarEvent.UserId = request.TargetUserId;
                        var targetUser = _context.Users.First(u => u.Id == request.TargetUserId);
                        calendarEvent.Text = targetUser.UserName;
                        _context.Requests.Remove(request);
                        _context.SaveChanges();
                    }
                }
            }

            return Index();
        }

        public IActionResult DeleteEventRequest(string requestId)
        {
            if (int.TryParse(requestId, out int rid))
            {
                var request = _context.Requests.First(r => r.Id == rid);
                if (int.TryParse(request.EventId, out int evId))
                {
                    _context.Requests.Remove(request);
                    _context.SaveChanges();
                }
            }

            return Index();
        }

        private RequestsViewModel PrepareModel()
        {
            var requestsViewModel = new RequestsViewModel();
            CreateRequestsList(requestsViewModel);
            return requestsViewModel;
        }

        private void CreateRequestsList(RequestsViewModel requestsViewModel)
        {
            List<RequestModel> requests = new List<RequestModel>();
            var currentUser = _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)).Result;
            if (currentUser != null)
            {
                if (currentUser.IsSystemAdmin)
                {
                    requests = _context.Requests.ToList();
                }
                else
                {
                    requests = _context.Requests.Where(r => r.TargetedUserId == currentUser.Id || r.TargetUserId == currentUser.Id).ToList();
                }
            }
            
            var requestsList = new List<RequestViewModel>();
            if (requests.Count > 0)
            {
                foreach (var request in requests)
                {
                    var targetUserName = _userManager.FindByIdAsync(request.TargetUserId).Result.UserName;
                    var targetedUserName = _userManager.FindByIdAsync(request.TargetedUserId).Result.UserName;

                    requestsList.Add(new RequestViewModel()
                    {
                        TargetedUser = targetedUserName,
                        TargetUser = targetUserName,
                        RequestId = request.Id.ToString(),
                        Date = request.Date
                    });

                    requestsViewModel.RequestsList = requestsList;
                }
            }
            else
            {
                requestsViewModel.RequestsList = requestsList;
            }
        }
    }
}
