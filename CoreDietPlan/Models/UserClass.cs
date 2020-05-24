using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.ML;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CoreDietPlan.Models
{
    /*
     * This class holds all methods involving users of the application
     */
    public class UserClass
    {
        string username;
        DietPlanDBContext db = new DietPlanDBContext();

        //create an object of this class passing the username of the user as a parameter to the constructor
        public UserClass(string username)
        {
            this.username = username;

        }

        public void NewUserConfig(DietUsers dietUsers, double UserWc, double WeightPounds, double HeightInches, double UserWeight, double UserHeight)
        {
            //hash the password
            MD5 md5Hash = MD5.Create();
            string hashPassword = GetMd5Hash(md5Hash, dietUsers.UserPassword);
            dietUsers.UserPassword = hashPassword;
            dietUsers.Status = "Active";

            //Add a record to ProgressTracker Table
            ProgressTracker FirstPRogress = new ProgressTracker();
            double BMI = UserWeight / (UserHeight * UserHeight);
            FirstPRogress.TrackerId = Guid.NewGuid();
            FirstPRogress.UserName = dietUsers.UserName;
            FirstPRogress.Wc = Convert.ToDouble(UserWc);
            FirstPRogress.Bmi = BMI;
            FirstPRogress.Month = 1;
            FirstPRogress.MonthlyHeight = UserHeight;
            FirstPRogress.MonthlyWeight = UserWeight;
            FirstPRogress.DateEntered = DateTime.Today;
            double? BMR;
            int WCRange = 0;

            if (dietUsers.UserGender == "Male")
            {
                BMR = 66 + (6.23 * WeightPounds) + (12.7 * HeightInches) - (6.8 * dietUsers.UserAge);

                if (UserWc < 94)
                    WCRange = 1;
                else if (UserWc < 102)
                    WCRange = 2;
                else
                    WCRange = 3;

            }
            else
            {
                BMR = 655 + (4.35 * WeightPounds) + (4.7 * HeightInches) - (4.7 * dietUsers.UserAge);
                if (UserWc < 80)
                    WCRange = 1;
                else if (UserWc < 88)
                    WCRange = 2;
                else
                    WCRange = 3;

            }
            FirstPRogress.Bmr = (double)BMR;
            db.Add(FirstPRogress);

            float age = (float)Convert.ToDouble(dietUsers.UserAge);



            float wc = (float)Convert.ToDouble(UserWc);
            //send user's details to stochastic class to get weight category
            dietUsers.UserWeightCategory = WeightClassification.returnClassification((float)BMI, WCRange);
            dietUsers.NewUser = true;
            db.Add(dietUsers);
            db.SaveChanges();

        }

        //function for hashing the password
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();
            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        //Method for checking if password input and from database is the same
        public static bool VerifyMd5Hash( string input, string hash)
        {
            MD5 md5Hash = MD5.Create();

            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task uploadPicture(List<IFormFile> files)
        {
           

            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0 && username != null)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                        ImportPic.Mains(filePath, username);
                        
                        break;
                    }
                }
             
            }
         //   dietuser.NewUser = false;
          //  db.Update(dietuser);
           // db.SaveChanges();

        }

        public  List<double> CalConsumption()
        {
            double? totalCal = 0;
            double? totalP = 0;
            double? totalCarb = 0;
            double? totalFats = 0;
            var FoodConsump = db.UserDailyConsumption.Where(z => z.Username.ToLower() == username.ToLower() && z.RecordedDate == DateTime.Today).ToList();
            var foods = db.FoodData.ToList();
            foreach (var consump in FoodConsump)
            {
                totalCal += foods.Where(x => x.Id == consump.ConsumptionId).Select(z => z.Calorie *consump.ConsumptionQuantity).FirstOrDefault();
                totalP += foods.Where(x => x.Id == consump.ConsumptionId).Select(z => z.Protein * consump.ConsumptionQuantity).FirstOrDefault();
                totalCarb += foods.Where(x => x.Id == consump.ConsumptionId).Select(z => z.Carbs * consump.ConsumptionQuantity).FirstOrDefault();
                totalFats += foods.Where(x => x.Id == consump.ConsumptionId).Select(z => z.Fat * consump.ConsumptionQuantity).FirstOrDefault();

            }
            List<double> macro = new List<double>();

            macro.Add(Math.Round((double)totalCal, 2));
            macro.Add(Math.Round((double)totalP, 2));
            macro.Add(Math.Round((double)totalCarb, 2));
            macro.Add(Math.Round((double)totalFats, 2));

            return macro;
        }

    }
}
