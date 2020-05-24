using System;
using System.Collections.Generic;

namespace CoreDietPlan.Models
{
    public partial class FoodData
    {
        public string Food { get; set; }
        public double? Protein { get; set; }
        public double? Carbs { get; set; }
        public double? Fat { get; set; }
        public double? Fiber { get; set; }
        public string FoodImg { get; set; }
        public int? Fgroup { get; set; }
        public string Serving { get; set; }
        public int? ServingQty { get; set; }
        public double? Calorie { get; set; }
        public int Id { get; set; }
    }
}
