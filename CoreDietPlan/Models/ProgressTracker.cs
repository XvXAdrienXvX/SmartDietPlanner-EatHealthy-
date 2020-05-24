using System;
using System.Collections.Generic;

namespace CoreDietPlan.Models
{
    public partial class ProgressTracker
    {
        public Guid TrackerId { get; set; }
        public int Month { get; set; }
        public double Bmi { get; set; }
        public double Wc { get; set; }
        public string UserName { get; set; }
        public double? MonthlyWeight { get; set; }
        public double? MonthlyHeight { get; set; }
        public DateTime? DateEntered { get; set; }
        public double Bmr { get; set; }
        public double Tdee { get; set; }
        public string ActivityLevel { get; set; }
    }
}
