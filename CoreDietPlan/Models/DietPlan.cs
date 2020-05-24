using System;
using System.Collections.Generic;

namespace CoreDietPlan.Models
{
    public partial class DietPlan
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string FoodId { get; set; }
        public DateTime? Date { get; set; }
        public string Meals { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }
        public int? Id { get; set; }
        public int DietPlanId { get; set; }
    }
}
