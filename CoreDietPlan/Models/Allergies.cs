using System;
using System.Collections.Generic;

namespace CoreDietPlan.Models
{
    public partial class Allergies
    {
        public Guid RecordId { get; set; }
        public string UserName { get; set; }
        public string AllergiesList { get; set; }
        public bool? IsAllergic { get; set; }
    }
}
