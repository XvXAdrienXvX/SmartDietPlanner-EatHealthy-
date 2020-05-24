using System;
using System.Collections.Generic;

namespace CoreDietPlan.Models
{
    public partial class UserMacros
    {
        public string Username { get; set; }
        public string Bmi { get; set; }
        public double Protein { get; set; }
        public double Carb { get; set; }
        public double Fat { get; set; }
        public double Calorie { get; set; }
        public int RecordId { get; set; }
        public DateTime? RecordedDate { get; set; }
    }
}
