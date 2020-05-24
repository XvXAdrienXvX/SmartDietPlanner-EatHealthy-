using System;
using System.Collections.Generic;

namespace CoreDietPlan.Models
{
    public partial class Preferences
    {
        public Guid RecordId { get; set; }
        public string Username { get; set; }
        public string PreferencesList { get; set; }
    }
}
