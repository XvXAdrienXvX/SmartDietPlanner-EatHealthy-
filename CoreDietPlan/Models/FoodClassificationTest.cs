using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDietPlan.Models
{
    public class FoodClassificationTest
    {  
        public static List<FoodCustomClass> FoodClassification()
        {
            List<FoodCustomClass> meal = new List<FoodCustomClass> { };
            DietPlanDBContext context = new DietPlanDBContext();
            var nutrients = context.FoodData.ToList();
            //Initiate the api request
            //& get the datatable returned from the function 
            //DataTable dt = new APICall().PostRequest("https://trackapi.nutritionix.com/v2/natural/nutrients");

            //Iterate tgrough the dataTable and retrieve each food with their respective macronutrients
            //foreach (DataRow row in dt.Rows)
            //{


            //    meal.Add(
            //        new FoodCustomClass
            //        {
            //            ID = Int32.Parse(row["ID"].ToString()),
            //            Food = row["Food"].ToString(),
            //            Total_Protein = float.Parse(row["Protein"].ToString(), CultureInfo.InvariantCulture.NumberFormat),
            //            Total_Carb = float.Parse(row["Carb"].ToString(), CultureInfo.InvariantCulture.NumberFormat),
            //            Total_Fat = float.Parse(row["Fat"].ToString(), CultureInfo.InvariantCulture.NumberFormat),
            //            Num_Calorie = float.Parse(row["Calorie"].ToString(), CultureInfo.InvariantCulture.NumberFormat),
            //            Fiber = float.Parse(row["Fiber"].ToString(), CultureInfo.InvariantCulture.NumberFormat),
            //            Serving = row["serving"].ToString(),
            //            //ServingQty= Int32.Parse(row["servingQty"].ToString()),
            //            img = row["img"].ToString(),
            //            fgroup = Int32.Parse(row["fgroup"].ToString())

            //        });
            //}
            foreach (var n in nutrients)
            {
                meal.Add(
                   new FoodCustomClass
                   {
                       ID = Int32.Parse(n.Id.ToString()),
                       Food = n.Food.ToString(),
                       Total_Protein = float.Parse(n.Protein.ToString(), CultureInfo.InvariantCulture.NumberFormat),
                       Total_Carb = float.Parse(n.Carbs.ToString(), CultureInfo.InvariantCulture.NumberFormat),
                       Total_Fat = float.Parse(n.Fat.ToString(), CultureInfo.InvariantCulture.NumberFormat),
                       Num_Calorie = float.Parse(n.Calorie.ToString(), CultureInfo.InvariantCulture.NumberFormat),
                       Fiber = float.Parse(n.Fiber.ToString(), CultureInfo.InvariantCulture.NumberFormat),
                       Serving = n.Serving.ToString(),
                       ServingQty = Int32.Parse(n.ServingQty.ToString()),
                       img = n.FoodImg.ToString(),
                       fgroup = Int32.Parse(n.Fgroup.ToString())

                   });
            }
            return meal;
        }

    }
}
