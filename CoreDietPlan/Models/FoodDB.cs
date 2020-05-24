using System;
using System.Collections.Generic;

namespace CoreDietPlan.Models
{
    public partial class FoodDb
    {
        public int Id { get; set; }
        public int? ServingQty { get; set; }
        public string Serving { get; set; }
        public string Fgroup { get; set; }
        public string Food { get; set; }
        public string Fcategory { get; set; }
        public string MealTime { get; set; }
    }
}
