using Accord;
using CoreDietPlan;
using CoreDietPlan.Models;
using Microsoft.AspNetCore.Html;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using Xunit;

namespace TestingDietPlanUnits
{
    public class DietTest
    {
        DietPlanDBContext db = new DietPlanDBContext();
        string Username = "AdrienV";
        [Fact]
        public void APITest()
        {
            //testing nutitionix api response
            var food = new DietPlanFunctions();
            Assert.True(food.testAPI("https://trackapi.nutritionix.com/v2/natural/nutrients", "oatmeal"));
        }
        [Fact]
        public void GetDetails()
        {
            //get BMI to pass to decision tree and calculations
            var food = new DietPlanFunctions();
            Assert.True(food.GetUserDetails());
        }
        [Fact]
        public void PositiveFoodsTest()
        {
            //testing if only foods with decision='positive' are returned
            var food = new DietPlanFunctions();
            Assert.True(food.checkFood());
        }
        [Fact]
        public void CalorieCalculationTest()
        {
            //testing if calculations for TDEE and Calorie based on BMI
            var food = new DietPlanFunctions();
            Assert.True(food.Calorie());
        }
    }
}

