using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CoreDietPlan.Models
{
    public class DietPlanFunctions
    {
        DietPlanDBContext db = new DietPlanDBContext();
        public static List<Meal> array = new List<Meal> { };
        public static List<Meal> remove = new List<Meal> { };
        //public static double? calorie = 0;
        public static double protein = 0, carbs = 0, fat = 0, weight = 60;
        public static double WeightKg = 2.205;

        string Username="AdrienV";
        //string BMI="obese";
        //string activity="sedentary";
        //double? TDEE=3000;
        public DietPlanFunctions()
        {          
            //BMI = db.DietUsers.Where(z => z.UserName.ToLower() == Username.ToLower()).Select(x => x.UserWeightCategory).FirstOrDefault();
            //activity = db.ProgressTracker.Where(z => z.UserName.ToLower() == Username.ToLower()).Select(x => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.ActivityLevel.ToLower())).FirstOrDefault();
            //TDEE = db.ProgressTracker.Where(z => z.UserName.ToLower() == Username.ToLower()).Select(x => x.Tdee).FirstOrDefault();
        }
      
        public bool testAPI(string url,string foods)
        {
           string nutrient = "";
            bool success;
           var queries = new List<KeyValuePair<string, string>>();
          
           
            queries.Add(new KeyValuePair<string, string>("query", foods));


            HttpContent q = new FormUrlEncodedContent(queries);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-app-id", "11bddcae");
                client.DefaultRequestHeaders.Add("x-app-key", "285fb98d493d0404238960830ecc5fb8");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.PostAsync(url, q).Result;
                var content = response.Content;

                RootObject list = JsonConvert.DeserializeObject<RootObject>(content.ReadAsStringAsync().Result);

                foreach (Food item in list.foods)
                {
                    nutrient = item.food_name + ":" + item.nf_protein.ToString();
                   
                }
                if (nutrient != null)
                {
                    success = true;
                }
                else
                {
                    success = false;
                }
                return success;
            }
        }
        public bool GetUserDetails()
        {    

            try
            {
                string bmi = db.DietUsers.Where(z => z.UserName.ToLower() == "adrienv").Select(x => x.UserWeightCategory).FirstOrDefault();
                string activity = db.ProgressTracker.Where(z => z.UserName.ToLower() == "adrienv").Select(x => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.ActivityLevel.ToLower())).FirstOrDefault();
                double? tdee = db.ProgressTracker.Where(z => z.UserName.ToLower() == "adrienv").Select(x => x.Tdee).FirstOrDefault();
                return true;

            }
            catch (SqlException)
            {
                return false;
            }

        }
        public bool checkFood()
        {
            bool check=true;
           List<Meal>positive = new DisplayDiet().GetMeals().ToList();
           foreach(var i in positive)
            {
                if(i.decision.Equals("positive"))
                {
                    check = true;
                }
                else
                {
                    check = false;
                }
            }
            return check;
        }
        public bool Calorie()
        {
            try
            {
                //string bmi = db.DietUsers.Where(z => z.UserName.ToLower() == "adrienv").Select(x => x.UserWeightCategory).FirstOrDefault();
                //string activity = db.ProgressTracker.Where(z => z.UserName.ToLower() == "adrienv").Select(x => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.ActivityLevel.ToLower())).FirstOrDefault();
                //double? tdee = db.ProgressTracker.Where(z => z.UserName.ToLower() == "adrienv").Select(x => x.Tdee).FirstOrDefault();
                string bmi = "normal";
                double? tdee = 2000;
                if (bmi.Equals("normal"))
                {
                    double? calorie = tdee;
                    return true;
                }
                else if (bmi.Equals("overweight"))
                {
                    double? calorie = 0.10 * tdee;
                    return true;
                }
                else if (bmi.Equals("obese"))
                {
                    double?calorie = 0.15 * tdee;
                    return true;
                }
                else if (bmi.Equals("morbid"))
                {
                    double? calorie = 0.25 * tdee;
                    return true;
                }
                else
                {
                    return false;
                }
               

            }
            catch (SqlException)
            {
                return false;
            }
        }
       
    }
}
