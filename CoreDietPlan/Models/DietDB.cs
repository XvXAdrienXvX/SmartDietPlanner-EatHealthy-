using System;
using System.Collections.Generic;

namespace CoreDietPlan.Models
{
    public partial class DietDb
    {
        public DateTime? Date { get; set; }
        public string MealTime { get; set; }
        public string Meals { get; set; }
        public string Serving { get; set; }
        public int? PrefId { get; set; }
        public string Username { get; set; }
        public int FoodId { get; set; }
    }
}
