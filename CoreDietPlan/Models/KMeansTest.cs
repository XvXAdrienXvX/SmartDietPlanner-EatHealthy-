using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDietPlan.Models
{
    static class KMeansTest
    {
        internal static readonly MealClass meal = new MealClass
        {
           food="chicken",
           carb=0.40f,
           protein=0.60f,
           fat=0.30f,
           calorie=150f
        };
    }
}
