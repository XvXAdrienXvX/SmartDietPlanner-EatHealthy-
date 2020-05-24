using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.ML.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDietPlan.Models
{
    public class DisplayDiet
    {
        DietPlanDBContext db = new DietPlanDBContext();
        public static List<Meal> finalBreakfast = new List<Meal> { };
        public static List<Meal> finalLunch = new List<Meal> { };
        public static List<Meal> finalDinner = new List<Meal> { };
        Dictionary<string, List<Meal>> Plate = new Dictionary<string, List<Meal>>();
        //Dictionary<string, Dictionary<string, List<Meal>>> DietPlan = new Dictionary<string, Dictionary<string, List<Meal>>>();
        StringBuilder sb = new StringBuilder();
        public static List<Meal> positive = new List<Meal> { };
        public static List<Meal> selected = new List<Meal> { };
        public static List<float> Total_selected = new List<float>();
        public static List<User> users = new List<User> { };
        public User macros = new User();
        public static User usr = new User();
        public static string User = "";
        //public static int tdee = 2000;
        //string bmi = "", activity = "Sedentary";
        public static double? tdee = 0;
        public static string bmi = "", activity = "";
        public static double? calorie = 0;
        public static float protein = 0f, carbs = 0f, fat = 0f, weight = 60f;
        public static float WeightKg = 2.205f;
        public static float carbCount = 0f;
        public float proteinCount = 0f;
        public static float fatCount = 0f;
        public string allergy = "";
        public bool? veg;
        public List<Meal> GetFoods(List<Meal> p)
        {
            positive = p;

            return positive;
        }
        public List<User> GetUser(List<User> u)
        {
            users = u;

            return users;
        }
        //public List<Meal> GetFoodsSelected(List<Meal> f)
        //{
        //    selected = f;

        //    return selected;
        //}
        public Dictionary<string, List<Meal>> CreateDiet()
        {
            positive = GetFoods(positive);
            users = GetUser(users);
            //selected = GetFoodsSelected(selected);
            foreach (User u in users)
            {
                User = u.user;
                bmi = u.bmi;
                activity = u.activity;
                tdee = u.tdee;
                allergy = u.allergy;
                veg = u.veg;
            }
            //User = users.ToList().Select(x=>x.user).FirstOrDefault();
            //bmi = users.ToList().Where(x => x.user == User).Select(y => y.bmi).FirstOrDefault();

            string[] mealTime = new string[] { "Breakfast", "Lunch", "Dinner" };
            Plate.Add("Breakfast", new List<Meal> { });
            Plate.Add("Lunch", new List<Meal> { });
            Plate.Add("Dinner", new List<Meal> { });
            //for (int i=0;i<NumMeals;i++)
            //{
            //    Plate.Add(mealTime[i], new List<Meal> { });
            //}


            //var pair in meals.Keys.Zip(list.foods, Tuple.Create)
            List<string> shellfish = new List<string>() { "crab", "lobster", "crayfish", "shrimp", "prawn", "mussel", "oyster", "scallop", "clam", "squid", "octopus" };
            List<string> nuts = new List<string>() { "almonds", "pistachios", "walnuts", "cashews", "pecans", "hazelnuts", "peanuts" };
            List<string> fish = new List<string>() { "salmon", "tuna", "rainbow trout", "pacific halibut", "mackerel", "cod", "sardines", "herring" };
            //List<string> allergies = allergy.Split(",").ToList();

            //foreach(var i in allergies)
            //{
            //if (i.Equals("fish", StringComparison.OrdinalIgnoreCase))
            //{
            //    positive = positive.Where(x => !fish.Any(y => x.food.Contains(y, StringComparison.OrdinalIgnoreCase))).ToList();
            //}
            //if (i.Equals("shellfish", StringComparison.OrdinalIgnoreCase))
            //{
            //    positive = positive.Where(x => !shellfish.Any(y => x.food.Contains(y, StringComparison.OrdinalIgnoreCase))).ToList();
            //}
            //if (i.Equals("nuts", StringComparison.OrdinalIgnoreCase))
            //{
            //    positive = positive.Where(x => !nuts.Any(y => x.food.Contains(y, StringComparison.OrdinalIgnoreCase))).ToList();
            //}
            //if (i.Equals("milk", StringComparison.OrdinalIgnoreCase))
            //{
            //    positive = positive.Where(x => x.fgroup!=1).ToList();
            //}
            //if (i.Equals("eggs", StringComparison.OrdinalIgnoreCase))
            //{
            //    positive = positive.Where(x =>!x.food.Contains("eggs", StringComparison.OrdinalIgnoreCase)).ToList();
            //}
            //if (i.Equals("soy", StringComparison.OrdinalIgnoreCase))
            //{
            //    positive = positive.Where(x => !x.food.Contains("soy", StringComparison.OrdinalIgnoreCase)).ToList();
            //}
            //}
            //if(veg==true)
            //{
            //    positive = positive.Where(x => x.fgroup != 2).ToList();
            //}

            CalculateMacros(bmi, activity, tdee, weight, WeightKg);
            macros = CalculateMacros(bmi, activity, tdee, weight, WeightKg);
            usr = macros;
            setMacros(usr);
            SaveToDB(User,macros);
            /*To iterate through Plate in DietPlanner.cshtml & display the foods,
             send Plate as param in ProposedDiet
             In ProposedDiet method replace dict with Plate
             comment Dictionary dict
           */
            ProposedDiet(macros, Plate);

            return Plate;

        }
        public User setMacros(User usr)
        {
            return usr;
        }
        public User getMacros()
        {
            return usr;
        }
        public void SaveToDB(string User, User u)
        {
            //using (var db = new DietPlanDBContext())
            //{

            //    var macros = new UserMacros { Username = User, Bmi = u.bmi, Protein = u.protein, Carb = u.carbs, Fat = u.fat, Calorie = (float)u.cal };

            //    db.UserMacros.Add(macros);
            //    db.SaveChanges();
            //}
        }
        public List<Meal> GetMeals()
        {

            return positive;
        }
        public List<Meal> FilterMeals()
        {
            positive = positive.Where(x => x.ncalorie < 100).ToList();
            return positive;
        }
        public List<User> GetUsr()
        {

            foreach (var u in users)
            {

                u.user = User;
                u.bmi = bmi;
                u.cal = (float)calorie;
                u.protein = protein;
                u.fat = fat;
                u.carbs = carbs;
            }
            return users;
        }
        //public List<float> GetSelected()
        //{

        //    foreach (var s in selected)
        //    {
        //        carbCount += s.ncarb;
        //        proteinCount += s.nprotein;
        //        fatCount += s.nfat;
        //    }
        //    Total_selected.Add(carbCount);
        //    Total_selected.Add(proteinCount);
        //    Total_selected.Add(fatCount);
        //    return Total_selected;
        //}
        public User CalculateMacros(string bmi, string activity, double? tdee, float weight, float WeightKg)
        {
            var result = new User();
            if (bmi.Equals("normal"))
            {
                result = new Link().normal(bmi, activity, tdee, weight, WeightKg);

            }
            else if (bmi.Equals("overweight"))
            {
                result = new Link().overweight(bmi, activity, tdee, weight, WeightKg);
            }
            else if (bmi.Equals("obese"))
            {
                result = new Link().obese(bmi, activity, tdee, weight, WeightKg);
            }
            else if (bmi.Equals("morbid"))
            {
                result = new Link().morbid(bmi, activity, tdee, weight, WeightKg);
            }
            return result;
        }
        public Dictionary<string, List<Meal>> ProposedDiet(User macros,Dictionary<string, List<Meal>> Plate)
        {
            Random rnd = new Random();
            double AllocatedProtein = macros.protein;
            double AllocatedCarb = macros.carbs;
            double AllocatedFat = macros.fat;
            double AllocatedCalorie = (double)macros.cal;
          //Dictionary<string, List<Meal>> dict = new Dictionary<string, List<Meal>> { };
          //dict.Add("Breakfast", new List<Meal> { });
          //dict.Add("Lunch", new List<Meal> { });
          //dict.Add("Dinner", new List<Meal> { });



            //string[] MealTimeArray = { "Breakfast", "Lunch", "Dinner" };

            var userPrefs = db.Preferences.Where(z => z.Username.ToLower() == "testing11").Select(y => y.PreferencesList.ToLower()).FirstOrDefault();
            string[] prefs = userPrefs.Split(',');
            //var finalMeal = "";
            double? ProteinT = 0;
            double? CarbT = 0;
            double? CalT = 0;
            double? FatT = 0;

            List<Combinations> Breakfast = new List<Combinations>();
            List<Combinations> Lunch = new List<Combinations>();
            List<Combinations> Dinner = new List<Combinations>();

            foreach (var m in Plate)
            {
                var MealList = db.Combinations.Where(x => x.MealTime.ToLower().Contains(m.Key.ToLower())).ToList();
                foreach (var meal in MealList)
                {
                    if (prefs.Any(meal.Preferences.ToLower().Contains))
                    {
                        ProteinT = 0; CalT = 0; FatT = 0; CarbT = 0;

                        string[] MealIDs = meal.Combinations1.Split(',');
                        foreach (var IDs in MealIDs)
                        {
                            var thisMeal = db.FoodData.Where(z => z.Id == int.Parse(IDs)).First();
                            ProteinT += thisMeal.Protein;
                            CalT += thisMeal.Calorie;
                            FatT += thisMeal.Fat;
                            CarbT += thisMeal.Carbs;

                        }
                        if (m.Key == "Breakfast")
                        {

                            if (ProteinT < AllocatedProtein && CarbT < AllocatedCarb && FatT < AllocatedFat && CalT < AllocatedCalorie)
                                Breakfast.Add(meal);
                        }
                        else if (m.Key == "Lunch")
                        {
                            if (ProteinT < AllocatedProtein && CarbT < AllocatedCarb && FatT < AllocatedFat && CalT < AllocatedCalorie)
                                Lunch.Add(meal);
                        }

                        else
                        {
                            if (ProteinT < AllocatedProtein && CarbT < AllocatedCarb && FatT < AllocatedFat && CalT < AllocatedCalorie)
                                Dinner.Add(meal);
                        }

                    }


                }
                if (m.Key == "Breakfast")
                {
                    int ran = rnd.Next(Breakfast.Count);
                    //finalMeal += Breakfast[ran].Combinations1 + ";";

                    var numbers = Breakfast[ran].Combinations1.Split(',').Select(Int32.Parse).ToList();
                    finalBreakfast = db.FoodData.Where(x => numbers.Any(a => a == x.Id)).Select(x => new Meal { food = x.Food, protein = x.Protein.ToString(), carb = x.Carbs.ToString(), fat = x.Fat.ToString(), calorie = x.Calorie.ToString(), fiber = x.Fiber.ToString(), img = x.FoodImg, serving = x.Serving, servingQty = x.ServingQty.ToString() }).ToList();
                    //Plate["Breakfast"].Add(finalBreakfast);
                }
                else if (m.Key == "Lunch")
                {
                    int ran = rnd.Next(Lunch.Count);
                    //finalMeal += Lunch[ran].Combinations1 + ";";

                    var numbers = Breakfast[ran].Combinations1.Split(',').Select(Int32.Parse).ToList();
                    finalLunch = db.FoodData.Where(x => numbers.Any(a => a == x.Id)).Select(x => new Meal { food = x.Food, protein = x.Protein.ToString(), carb = x.Carbs.ToString(), fat = x.Fat.ToString(), calorie = x.Calorie.ToString(), fiber = x.Fiber.ToString(), img = x.FoodImg, serving = x.Serving, servingQty = x.ServingQty.ToString() }).ToList();
                    //Plate["Lunch"].Add(finalLunch);
                }
                else
                {
                    int ran = rnd.Next(Dinner.Count);
                    //finalMeal += Dinner[ran].Combinations1;

                    var numbers = Dinner[ran].Combinations1.Split(',').Select(Int32.Parse).ToList();
                    finalDinner = db.FoodData.Where(x => numbers.Any(a => a == x.Id)).Select(x => new Meal { food = x.Food, protein = x.Protein.ToString(), carb = x.Carbs.ToString(), fat = x.Fat.ToString(), calorie = x.Calorie.ToString(), fiber = x.Fiber.ToString(), img = x.FoodImg, serving = x.Serving, servingQty = x.ServingQty.ToString() }).ToList();
                    //Plate["Dinner"].Add(finalDinner);
                }
            }
            //List<Meal> final = db.FoodData.Where(x => finalMeal.Any(a => a == x.Id)).Select(x => x.Food).ToList();
            BreakfastMeal(finalBreakfast);
            LunchMeal(finalLunch);
            DinnerMeal(finalDinner);
            foreach (Meal m in finalBreakfast)
            {
                Plate["Breakfast"].Add(new Meal { food = m.food, protein = m.protein.ToString(), carb = m.carb.ToString(), fat = m.fat.ToString(), calorie = m.calorie.ToString(), fiber = m.fiber.ToString(), img = m.img, serving = m.serving, servingQty = m.servingQty.ToString() });
            }
            foreach (Meal m in finalLunch)
            {
                Plate["Lunch"].Add(new Meal { food = m.food, protein = m.protein.ToString(), carb = m.carb.ToString(), fat = m.fat.ToString(), calorie = m.calorie.ToString(), fiber = m.fiber.ToString(), img = m.img, serving = m.serving, servingQty = m.servingQty.ToString() });
            }
            foreach (Meal m in finalDinner)
            {
                Plate["Dinner"].Add(new Meal { food = m.food, protein = m.protein.ToString(), carb = m.carb.ToString(), fat = m.fat.ToString(), calorie = m.calorie.ToString(), fiber = m.fiber.ToString(), img = m.img, serving = m.serving, servingQty = m.servingQty.ToString() });
            }
            return Plate;
        }
        public List<Meal> BreakfastMeal(List<Meal> finalBreakfast)
        {
            return finalBreakfast;
        }
        public List<Meal> LunchMeal(List<Meal> finalLunch)
        {
            return finalLunch;
        }
        public List<Meal> DinnerMeal(List<Meal> finalDinner)
        {
            return finalDinner;
        }
        public List<Meal> getBreakfastMeal()
        {
            return finalBreakfast;
        }
        public List<Meal> getLunchMeal()
        {
            return finalLunch;
        }
        public List<Meal> getDinnerMeal()
        {
            return finalDinner;
        }
        public Dictionary<string, List<Meal>> getDict()
        {
            Dictionary<string, List<Meal>> dict = new Dictionary<string, List<Meal>> { };
            dict.Add("Breakfast", new List<Meal> { });
            dict.Add("Lunch", new List<Meal> { });
            dict.Add("Dinner", new List<Meal> { });

            List<Meal> finalBreakfast = getBreakfastMeal();
            List<Meal> finalLunch = getLunchMeal();
            List<Meal> finalDinner = getDinnerMeal();
            foreach (Meal m in finalBreakfast)
            {
                dict["Breakfast"].Add(new Meal { food = m.food, protein = m.protein.ToString(), carb = m.carb.ToString(), fat = m.fat.ToString(), calorie = m.calorie.ToString(), fiber = m.fiber.ToString(), img = m.img, serving = m.serving, servingQty = m.servingQty.ToString() });
            }
            foreach (Meal m in finalLunch)
            {
                dict["Lunch"].Add(new Meal { food = m.food, protein = m.protein.ToString(), carb = m.carb.ToString(), fat = m.fat.ToString(), calorie = m.calorie.ToString(), fiber = m.fiber.ToString(), img = m.img, serving = m.serving, servingQty = m.servingQty.ToString() });
            }
            foreach (Meal m in finalDinner)
            {
                dict["Dinner"].Add(new Meal { food = m.food, protein = m.protein.ToString(), carb = m.carb.ToString(), fat = m.fat.ToString(), calorie = m.calorie.ToString(), fiber = m.fiber.ToString(), img = m.img, serving = m.serving, servingQty = m.servingQty.ToString() });
            }

            return dict;
        }
    }
}
