using Microsoft.ML.Runtime.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDietPlan.Models
{
    public class foodCSV
    {
        [Column(ordinal: "0")]
        public int FoodID { get; set; }

        [Column(ordinal: "1")]
        public string Food { get; set; }
    }
}
