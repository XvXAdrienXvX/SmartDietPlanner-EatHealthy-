using CoreDietPlan;
using CoreDietPlan.Models;
using Microsoft.AspNetCore.Html;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using Xunit;

namespace TestingDietPlanUnits
{
    public class UserUnitTesting
    {
        DietPlanDBContext db = new DietPlanDBContext();
        //[Fact]
        //public void WeightCategoryMLTest()
        //{
        //    float BMI = 27.52f;
        //    int WaistCircumference = 2;
        //    string expectedResult = "class1";
        //    Assert.Equal(expectedResult, WeightClassification.returnClassification (BMI, WaistCircumference));
        //}

        //[Fact]
        //public void CreateUserTest()
        //{
        //    DietUsers TestUser = new DietUsers();
        //    TestUser.UserName = "TestingUNITUser";
        //    TestUser.UserPassword = "123A";
        //    TestUser.UserGender = "Female";
        //    UserClass NewUser = new UserClass("TestingUNITUser");

        //    Assert.True(NewUser.NewUserConfig(TestUser, 22.2, 55, 5, 80, 32));
        //}

        [Fact]
        public void EncryptPWDTest()
        {
            string ExpectedEncryptedPWD = "b316f448bc5a84b75124a41f4e9e4109";
            string testPwd = "Testingpwd";
            MD5 EncryptionType = MD5.Create(); 

            Assert.Equal(ExpectedEncryptedPWD , UserClass.GetMd5Hash( EncryptionType, testPwd));
        }

        [Fact]
        public void DecryptPWDTest()
        {
            string ExpectedDecryptedPWD = "Testingpwd";
            string testEncryptedPwd = "b316f448bc5a84b75124a41f4e9e4109";
            MD5 EncryptionType = MD5.Create(); 

            Assert.True( UserClass.VerifyMd5Hash( EncryptionType, ExpectedDecryptedPWD, testEncryptedPwd));
        }

        [Fact]
        public void TimeToUpdateWeightTest()
        {
            //Testing the function which checks whether 30 days have passed since last update

            //User Testing0 last update exceeds 30 days
            DashboardFunctions TestUser = new DashboardFunctions("Testing0");

            //Getting Today's Date
            DateTime dt = DateTime.Now;

            //latest Month since update
            var TrackerList = db.ProgressTracker.ToList();
            var latestMonth = TrackerList.Where(z => z.UserName.ToLower() == "testing0").Max(x => x.Month); 

            Assert.True(TestUser.TimeOverdue(dt, latestMonth));
        }

        [Fact]
        public void DailyActivityTest()
        {
            //The unit being tested gets the daily activities recorded which is then displayed on dashboard

            //User Testing0 recorded "Badminton" as a daily activity performed
            UserClass TestUser = new UserClass("Testing0");


          //  Assert.Contains("Badminton" , TestUser.GetDailyActityList());
        }

        [Fact]
        public void AllergiesTest()
        {
            //The unit being tested retrives the list of allergies that user has selected prior to loading the dashboard

            //User Testing0 recorded "NUTS and SOY" as allegies
            DashboardFunctions TestUser = new DashboardFunctions("Testing0");


            Assert.Equal("NUTS,SOY" , TestUser.GetAllergiesList());
        }

        [Fact]
        public void PreferenceTest()
        {
            //The unit being tested retrives the list of preferences that user has selected prior to loading the dashboard

            //User Testing0 hasnt recorded any preferences
            DashboardFunctions TestUser = new DashboardFunctions("Testing0");


            Assert.Equal(null, TestUser.GetPreferencesList());
        }

        [Fact]
        public void UpdateAllergiesTest()
        {
            //The unit being tested updates the Allergies list as per User

            DashboardFunctions TestUser = new DashboardFunctions("Testing0");

            Assert.True(TestUser.UpdateAllergies("Dairy"));
        }

          [Fact]
        public void UpdatePreferencesTest()
        {
            //The unit being tested updates the Allergies list as per User

            DashboardFunctions TestUser = new DashboardFunctions("Testing0");

            Assert.True(TestUser.UpdatePreferences("Chicken"));
        }

    }
}
