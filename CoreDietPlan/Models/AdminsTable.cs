using System;
using System.Collections.Generic;

namespace CoreDietPlan.Models
{
    public partial class AdminsTable
    {
        public string AdminUsername { get; set; }
        public string AdminPassword { get; set; }
        public string AdminName { get; set; }
        public string Status { get; set; }
    }
}
