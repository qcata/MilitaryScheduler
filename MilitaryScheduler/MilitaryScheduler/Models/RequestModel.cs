using System.ComponentModel.DataAnnotations;

namespace MilitaryScheduler.Models
{
    public class RequestModel
    {
        [Key]
        public int Id { get; set; }
        public string TargetUserId { get; set; }
        public string TargetedUserId { get; set; }
        public string EventId { get; set; }
        public string Date { get; set; }
    }
}
