using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilitaryScheduler.Data;
using MilitaryScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MilitaryScheduler.Controllers
{
    [Produces("application/json")]
    [Route("api/Events")]
    public class EventsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context,
                                UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Events
        [HttpGet]
        public IEnumerable<CalendarEvent> GetEvents([FromQuery]DateTime start, [FromQuery]DateTime end)
        {
            var reponse = _context.Events.Where(e => !((e.End <= start) || (e.Start >= end))).ToList();
            foreach (var responseEvent in reponse)
            {
                if (responseEvent.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    responseEvent.Color = "#ea9999";
                }
            }
            return reponse;
        }

        [HttpGet("CreateChangeRequest/")]
        public string CreateChangeRequest(int eventId)
        {
            var targetedEvent = _context.Events.FirstOrDefault(e => e.Id == eventId);
            if (targetedEvent != null)
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var changeRequest = new RequestModel()
                {
                    EventId = eventId.ToString(),
                    TargetedUserId = targetedEvent.UserId,
                    TargetUserId = currentUserId
                };

                _context.Requests.Add(changeRequest);
                 _context.SaveChanges();
                return "Success";
            }
            else
            {
                return "Forbidden";
            }
        }

        // GET: api/Events/GetOverlapping
        [HttpGet("GetOverlapping/")]
        public string GetOverlappingEvents(string start)
        {
            var date = start.Substring(0, start.IndexOf('T'));
            DateTime startDate = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            var overlappingEvent = _context.Events.Where(e => (e.Start.Day == startDate.Day) && (e.Start.Year == startDate.Year)).FirstOrDefault();
            if (overlappingEvent != null)
            {
                var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _userManager.FindByIdAsync(userID).Result;
                var returnedObject = (overlappingEvent.Id, overlappingEvent.Text, user.IsSystemAdmin.ToString());
                return returnedObject.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var @event = await _context.Events.SingleOrDefaultAsync(m => m.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            return Ok(@event);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var @event = await _context.Events.SingleOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            if (@event.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                if (!_userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)).Result.IsSystemAdmin)
                {
                    return Ok("Forbidden");
                }
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return Ok(@event);
        }

    }

}
