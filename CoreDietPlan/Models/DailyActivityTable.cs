using System;
using System.Collections.Generic;

namespace CoreDietPlan.Models
{
    public partial class DailyActivityTable
    {
        public Guid RecordId { get; set; }
        public string Username { get; set; }
        public string Duration { get; set; }
        public DateTime? RecordedDate { get; set; }
        public string ActivityName { get; set; }
        public Guid? ActivityId { get; set; }
    }
}
