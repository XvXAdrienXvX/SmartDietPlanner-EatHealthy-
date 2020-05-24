using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDietPlan.Models
{
    public class Link
    {
        
        User u = new User();
        public User normal(string bmi, string activity,double? tdee, float weight, float WeightKg)
        {
            u.cal = tdee;
            u.bmi = bmi;
            u.activity = activity;
            if (activity.Equals("Sedentary"))
            {
                u.carbs = 1 * (weight * WeightKg);
                u.protein = 0.36f * (weight * WeightKg);
                u.fat = 0.20f * (weight * WeightKg);
            }
            else if (activity.Equals("Lightly"))
            {
                u.carbs = 2 * (weight * WeightKg);
                u.protein = 0.5f * (weight * WeightKg);
                u.fat = 0.30f * (weight * WeightKg);
            }
            else if (activity.Equals("Mild"))
            {
                u.carbs = 2.5f * (weight * WeightKg);
                u.protein = 0.5f * (weight * WeightKg);
                u.fat = 0.30f * (weight * WeightKg);
            }
            else if (activity.Equals("Very"))
            {
                u.carbs = 4 * (weight * WeightKg);
                u.protein = 0.8f * (weight * WeightKg);
                u.fat = 0.35f * (weight * WeightKg);
            }
            else if (activity.Equals("Extra"))
            {
                u.carbs = 5 * (weight * WeightKg);
                u.protein = 0.8f * (weight * WeightKg);
                u.fat = 0.35f * (weight * WeightKg);
            }
            return u;
        }
        public User overweight(string bmi, string activity, double? tdee, float weight, float WeightKg)
        {
            u.cal = 0.10 * tdee;
            if (activity.Equals("Sedentary"))
            {
                u.carbs = 1 * (weight * WeightKg);
                u.protein = 0.8f * (weight * WeightKg);
                u.fat = 0.20f * (weight * WeightKg);
            }
            else if (activity.Equals("Lightly"))
            {
                u.carbs = 2 * (weight * WeightKg);
                u.protein = 0.8f * (weight * WeightKg);
                u.fat = 0.20f * (weight * WeightKg);
            }
            else if (activity.Equals("Mild"))
            {
                u.carbs = 2 * (weight * WeightKg);
                u.protein = 0.8f * (weight * WeightKg);
                u.fat = 0.20f * (weight * WeightKg);
            }
            else if (activity.Equals("Very"))
            {
                u.carbs = 2 * (weight * WeightKg);
                u.protein = 0.8f * (weight * WeightKg);
                u.fat = 0.20f * (weight * WeightKg);
            }
            else if (activity.Equals("Extra"))
            {
                u.carbs = 2.5f * (weight * WeightKg);
                u.protein = 0.8f * (weight * WeightKg);
                u.fat = 0.20f * (weight * WeightKg);
            }
            return u;
        }
        public User obese(string bmi, string activity, double? tdee, float weight, float WeightKg)
        {
            u.cal = 0.15 * tdee;
            if (activity.Equals("Sedentary"))
            {
                u.carbs = 1 * (weight * WeightKg);
                u.protein = 0.8f * (weight * WeightKg);
                u.fat = 0.20f * (weight * WeightKg);
            }
            else if (activity.Equals("Lightly"))
            {
                u.carbs = 2 * (weight * WeightKg);
                u.protein = 0.8f * (weight * WeightKg);
                u.fat = 0.20f * (weight * WeightKg);
            }
            else if (activity.Equals("Mild"))
            {
                u.carbs = 2 * (weight * WeightKg);
                u.protein = 0.8f * (weight * WeightKg);
                u.fat = 0.20f * (weight * WeightKg);
            }
            else if (activity.Equals("Very"))
            {
                u.carbs = 2 * (weight * WeightKg);
                u.protein = 0.8f * (weight * WeightKg);
                u.fat = 0.20f * (weight * WeightKg);
            }
            else if (activity.Equals("Extra"))
            {
                u.carbs = 2.5f * (weight * WeightKg);
                u.protein = 0.8f * (weight * WeightKg);
                u.fat = 0.20f * (weight * WeightKg);
            }
            return u;
        }
        public User morbid(string bmi, string activity, double? tdee, float weight, float WeightKg)
        {
            u.cal = 0.25 * tdee;
            if (activity.Equals("Sedentary"))
            {
                u.carbs = 1 * (weight * WeightKg);
                u.protein = 0.8f * (weight * WeightKg);
                u.fat = 0.20f * (weight * WeightKg);
            }
            else if (activity.Equals("Lightly"))
            {
                u.carbs = 2 * (weight * WeightKg);
                u.protein = 0.8f * (weight * WeightKg);
                u.fat = 0.20f * (weight * WeightKg);
            }
            else if (activity.Equals("Mild"))
            {
                u.carbs = 2 * (weight * WeightKg);
                u.protein = 0.8f * (weight * WeightKg);
                u.fat = 0.20f * (weight * WeightKg);
            }
            else if (activity.Equals("Very"))
            {
                u.carbs = 2 * (weight * WeightKg);
                u.protein = 0.8f * (weight * WeightKg);
                u.fat = 0.20f * (weight * WeightKg);
            }
            else if (activity.Equals("Extra"))
            {
                u.carbs = 2.5f * (weight * WeightKg);
                u.protein = 0.8f * (weight * WeightKg);
                u.fat = 0.20f * (weight * WeightKg);
            }
            return u;
        }
    }
   
}
