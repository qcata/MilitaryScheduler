using Microsoft.AspNetCore.Mvc.Rendering;
using MilitaryScheduler.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MilitaryScheduler.ViewModels
{
    public class EventViewModel
    {
        [Display(Name = "Date:")]
        public DateTime Start { get; set; }
        [Display(Name = "Please select the User")]
        public ApplicationUser ApplicationUser { get; set; }
        public List<SelectListItem> ApplicationUsers { get; set; }
    }
}
