using System;
using System.Collections.Generic;

namespace CoreDietPlan.Models
{
    public partial class UserDailyConsumption
    {
        public Guid RecordId { get; set; }
        public int? ConsumptionId { get; set; }
        public string Username { get; set; }
        public DateTime? RecordedDate { get; set; }
        public int? ConsumptionQuantity { get; set; }
    }
}
