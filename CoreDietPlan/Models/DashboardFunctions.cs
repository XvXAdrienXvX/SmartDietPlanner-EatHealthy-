using Microsoft.AspNetCore.Html;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreDietPlan.Models
{
    public class DashboardFunctions
    {
        DietPlanDBContext db = new DietPlanDBContext();
        string Username;

       public DashboardFunctions(string username)
        {
            this.Username = username;
        }

        public HtmlString GetDailyActivityList()
        {
            var TodayDate = DateTime.Now.ToString("yyyy-MM-dd");
            string actList = "";
            var dailyAct = db.DailyActivityTable.Where(x => x.Username.ToLower() == Username.ToLower() && x.RecordedDate.ToString() == TodayDate).ToArray();

            for (int i = 0; i < dailyAct.Count(); i++)
            {
                actList += "<li title='Duration: " + dailyAct[i].Duration + " mins' style='cursor:default'><i class='menu-icon fas fa-circle'></i><a>" + db.ExercisesListTable.Where(z => z.ExerciseId == dailyAct[i].ActivityId).Select(x => x.ExerciseList).FirstOrDefault() + "</a></li>";

            }
            HtmlString htmlAct = new HtmlString(actList);
            return htmlAct;
        }


        public string GetAllergiesList()
        {
            var Allergy = db.Allergies.ToList();
            string choosenAllergies = Allergy.Where(z => z.UserName.ToLower() == Username.ToLower()).Select(x => x.AllergiesList).FirstOrDefault();
            return choosenAllergies;
        }


        public string GetPreferencesList()
        {
            var Preference = db.Preferences.ToList();
            string choosenPreferences = Preference.Where(z => z.Username.ToLower() == Username.ToLower()).Select(x => x.PreferencesList).FirstOrDefault();
            return choosenPreferences;
        }

        public bool UpdateAllergies(string Allergies)
        {
            try
            {
                var AllergiesToList = db.Allergies.ToList();
                var UserAllergies = AllergiesToList.Where(x => x.UserName.ToLower() == Username.ToLower()).FirstOrDefault();
                UserAllergies.AllergiesList = Allergies;
                db.Update(UserAllergies);
                db.SaveChanges();
                return true;

            }
            catch (SqlException)
            {
                return false;
            }
        }


        public bool UpdatePreferences(string Preferences)
        {
            try
            {
                var PreferencesToList = db.Preferences.ToList();
                var UserPreferences = PreferencesToList.Where(x => x.Username.ToLower() == Username.ToLower()).FirstOrDefault();
                UserPreferences.PreferencesList = Preferences;
                db.Update(UserPreferences);
                db.SaveChanges();
                return true;

            }
            catch (SqlException)
            {
                return false;
            }
        }



        public bool TimeOverdue(DateTime dt, int latestMonth)
        {
            var TrackerList = db.ProgressTracker.ToList();

            // get the date of latest recorded month
            var date = TrackerList.Where(z => z.UserName.ToLower() == Username.ToLower() && z.Month == latestMonth).Select(x => x.DateEntered).FirstOrDefault();
            //Get the date difference between "today" and last date entered
            TimeSpan MonthExceeded = (TimeSpan)(DateTime.Today - date);
            string Duration = MonthExceeded.Days.ToString() + "";
            Duration = Regex.Replace(Duration, "[:]", string.Empty);
            int DurationToInt = Int32.Parse(Duration); //Convert from date type to int type

            if (DurationToInt > 30)
            {
                return true;
            }
            else
                return false;

        }

        //public string FileUrl(bool? setPicture)
        //{
        //    CloudBlobContainer container;
        //    System.Uri newUri;// variable to save link to picture
        //    // Your code ------
        //    // Retrieve storage account from connection string.
        //    var storageCredentials = new StorageCredentials("dietplannerresources", "sgMzUOBlZ1HpKC08yZjdAoA3TrBvyeo+QDFWfkQ2gz8NRG2SrIsXbt3CTtCgLSwslbVwTM4w41KynDlZ44RXkA==");
        //    var cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
        //    // Create the blob client.
        //    CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();

        //    // Retrieve reference to a previously created container.
        //    container = blobClient.GetContainerReference("eathealthyimages");
        //    var readPolicy = new SharedAccessBlobPolicy()
        //    {
        //        Permissions = SharedAccessBlobPermissions.Read,
        //        SharedAccessExpiryTime = DateTime.UtcNow + TimeSpan.FromMinutes(20)
        //    };

        //    if (setPicture == true)
        //    {
        //        // Retrieve reference to a blob ie "picture.jpg".
        //        CloudBlockBlob blockBlob = container.GetBlockBlobReference(Username);
        //        newUri = new Uri(blockBlob.Uri.AbsoluteUri + blockBlob.GetSharedAccessSignature(readPolicy));
        //    }

        //    else
        //    {
        //        CloudBlockBlob blockBlob = container.GetBlockBlobReference("NoSetProfile.png");
        //        newUri = new Uri(blockBlob.Uri.AbsoluteUri + blockBlob.GetSharedAccessSignature(readPolicy));
        //    }
        //    return newUri.ToString();

        //}






    }
}
