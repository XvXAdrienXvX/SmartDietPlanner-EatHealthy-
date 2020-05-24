using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDietPlan.Models
{
    /*
   Category class created to store output from FoodClassification.cs
   E.g: food='chicken'
        protein='high'
        carb='low'
        fat='low'
        calorie='low'
  */
    public class Meal
    {
        public int PrefID { get; set; }
        public string CurrentUser { get; set; }
        public string food { get; set; }
        public string protein { get; set; }
        public string carb { get; set; }
        public string fat { get; set; }
        public string calorie { get; set; }
        public string fiber { get; set; }
        public float nprotein { get; set; }
        public float ncarb { get; set; }
        public float nfat { get; set; }
        public float ncalorie { get; set; }
        public float nfiber { get; set; }
        public string serving { get; set; }
        public string servingQty { get; set; }
        public string img { get; set; }
        public int fgroup { get; set; }
        public string decision { get; set; }
        public string meal_time { get; set; }
    }
}
