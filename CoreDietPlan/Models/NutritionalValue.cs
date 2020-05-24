using System;
using System.Collections.Generic;

namespace CoreDietPlan.Models
{
    public partial class NutritionalValue
    {
        public string Username { get; set; }
        public DateTime? Date { get; set; }
        public string FoodImg { get; set; }
        public string Food { get; set; }
        public string MealTime { get; set; }
        public double? Protein { get; set; }
        public double? Carbs { get; set; }
        public double? Fat { get; set; }
        public double? Fiber { get; set; }
        public int? Serving { get; set; }
        public double? Calorie { get; set; }
        public double? TotalCalorie { get; set; }
        public int FoodId { get; set; }
    }
}
