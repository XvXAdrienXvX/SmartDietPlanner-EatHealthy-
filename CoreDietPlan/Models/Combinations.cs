using System;
using System.Collections.Generic;

namespace CoreDietPlan.Models
{
    public partial class Combinations
    {
        public string Combinations1 { get; set; }
        public string Preferences { get; set; }
        public string MealTime { get; set; }
        public Guid RecordId { get; set; }
    }
}
