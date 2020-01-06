using Microsoft.AspNetCore.Identity;
using MilitaryScheduler.Models.Enums;

namespace MilitaryScheduler.Models
{
    public class ApplicationUser: IdentityUser
    {
        public MilitaryGrade MilitaryGrade { get; set; }
    }
}
