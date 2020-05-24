using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreDietPlan.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Html;
using System.Security.Cryptography;
using System.Data.SqlClient;

namespace CoreDietPlan.Controllers
{

    public class UserController : Controller
    {
        DietPlanDBContext db;
      
        public UserController(DietPlanDBContext context)
        {
            db = context;
        }
        //public IActionResult Index()
        //{
        //    //string username = "Testing1";

        //    //Apriori j = new Apriori();
        //    //var BreakFastDietplan = db.DietDB.Where(x => x.Username == username && x.MealTime.Contains("Breakfast")).Select(z => z.Meals).ToArray();
        //    //var LunchDietplan = db.DietDB.Where(x => x.Username == username && x.MealTime.Contains("Lunch")).Select(z => z.Meals).ToArray();
        //    //var DinnerDietplan = db.DietDB.Where(x => x.Username == username && x.MealTime == "Dinner").Select(z => z.Meals).ToArray();


        //    //string[] ss = BreakFastDietplan;

        //    //string[][] k = j.AprioriAI(BreakFastDietplan);

         
        //    return View();
        //}

        [HttpPost("upload")]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            string username = null;


                username = HttpContext.Session.GetString("Username");
                UserClass thisUser = new UserClass(username);
                await thisUser.uploadPicture(files);
                return RedirectToAction("Dashboard");

            

        }

        public IActionResult submittingAddtional(string Activity_Lev, string Preferences, bool IsAllergic, string Allergies, bool IsVeg)
        {
            
                var username = HttpContext.Session.GetString("Username");
                Allergies addAllergy = new Allergies();
                addAllergy.RecordId = Guid.NewGuid();
                addAllergy.UserName = username;
                addAllergy.IsAllergic = IsAllergic;
                addAllergy.AllergiesList = Allergies;
                //allergyList
                db.Add(addAllergy);

                Preferences AddPreferences = new Preferences();

                AddPreferences.RecordId = Guid.NewGuid();
                AddPreferences.Username = username;
                AddPreferences.PreferencesList = Preferences;
                db.Add(AddPreferences);

                double? TDEE = 0;
                var user = db.ProgressTracker.Where(x => x.UserName.ToLower() == username.ToLower()).FirstOrDefault();
                user.ActivityLevel = Activity_Lev;

                switch (Activity_Lev)
                {
                    case "sedentary": TDEE = user.Bmr * 1.2; break;

                    case "lightly": TDEE = user.Bmr * 1.375; break;

                    case "mild": TDEE = user.Bmr * 1.55; break;

                    case "very": TDEE = user.Bmr * 1.725; break;

                    case "extra": TDEE = user.Bmr * 1.9; break;

                }

                user.Tdee = (double)TDEE;
                db.Update(user);
                var dietuser = db.DietUsers.Find(username);
                  dietuser.NewUser = false;
                  db.Update(dietuser);

                db.SaveChanges();
                return RedirectToAction("Dashboard");
            }
     

      

        [HttpPost("UpdateAccount")]
        public  IActionResult UpdateAccount( [Bind("UserName,UserFirstName,UserLastName,UserAge,UserEmail,UserPassword")] DietUsers dietUsers)
        {
            try
            {
                string username = null;

                if (HttpContext.Session.GetString("LoggedIn") == null)
                {
                    return RedirectToAction("Index");

                }
                else

                {

                    username = HttpContext.Session.GetString("Username");
                    UserClass updateUser = new UserClass("Username");
                    var dietuser = db.DietUsers.Find(username);

                   
                    dietuser.UserFirstName = dietUsers.UserFirstName;
                    dietuser.UserLastName = dietUsers.UserLastName;
                    dietuser.UserEmail = dietUsers.UserEmail;
                    dietuser.UserAge = dietUsers.UserAge;
                    if (dietUsers.UserPassword != null)
                    {
                        MD5 md5Hash = MD5.Create();

                        string hash = UserClass.GetMd5Hash(md5Hash, dietUsers.UserPassword);
                        dietuser.UserPassword = hash;
                    }
                    db.Update(dietuser);
                    db.SaveChanges();



                    return RedirectToAction("Dashboard");
                }
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", new { controllerName = controller, actionName = action });

            }
        }

        private bool DietUsersExists(string id)
        {
            return db.DietUsers.Any(e => e.UserName.ToLower() == id.ToLower());
        }

        public string VerifyUsername(string Username)
        {
            var dbList = db.DietUsers.ToList();
            var UserNameExist = dbList.Where(z => z.UserName.ToLower() == Username.ToLower()).Select(x => x.UserName).FirstOrDefault();
            if (UserNameExist == null)
            {
                return "true";
            }
            else
                return "false";

        }
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Signup([Bind("UserName,UserAge,UserPassword,UserFirstName,UserLastName,UserEmail,UserGender")] DietUsers dietUsers, double UserWc, string WeightMetric, string HeightMetric, double UserWeight, double HeightMeter = 0, double HeightFeet = 0, double HeightInches = 0)
        {
            double WeightPound = 0;
            double HeightInch = 0;
            UserClass newUser = new UserClass(dietUsers.UserName);
            double height, weight;
            //Converting height to meters
            if (HeightMetric == "Feet")
            {
                height = HeightFeet * 12;
                height += HeightInches;
                height *= 0.025;
                HeightInch = height;

            }
            else
            { //if height was already set in meter
                height = HeightMeter;
                HeightInch = height * 39.3701;
            }
            if (WeightMetric == "kg")
            {
                //if weight was already set in kg
                WeightPound =  UserWeight * 2.20462;
                weight = UserWeight;
            }
            else
            {
                WeightPound = UserWeight;
                //converting weight into kg
                weight = UserWeight * 0.45;

            }
            newUser.NewUserConfig(dietUsers, UserWc, WeightPound, HeightInch, weight, height);

            //Start session for this user
            HttpContext.Session.SetString("LoggedIn", "True");
            HttpContext.Session.SetString("Username", dietUsers.UserName);
            //redirect to dashboard page
            return RedirectToAction("Dashboard");

        }


        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserFailedReason") == "Incorrect")
            {
                string errString = "<div class='alert alert-danger'><strong> Error </strong> Incorrect username or password entered  </div>";
                HtmlString errHtml = new HtmlString(errString);
                ViewBag.failedLogin = errHtml;
            }
            else if (HttpContext.Session.GetString("UserFailedReason") == "Suspended")
            {
                string errString = "<div class='alert alert-danger'><strong> Error </strong> Your account has been suspended, Try again in a while </div>";
                HtmlString errHtml = new HtmlString(errString);
                ViewBag.failedLogin = errHtml;

               
            }

            else
                ViewBag.failedLogin = "";

            return View();
        }

        [HttpPost]
        public IActionResult Login(string userID, string password)
        {
            try
            {
                UserClass LoginUser = new UserClass(userID);

                var dbList = db.DietUsers.ToList();
                //getting the password of this user from database
                var pwd = dbList.Where(z => z.UserName.ToLower() == userID.ToLower()).Select(x => x.UserPassword).FirstOrDefault();

                //method for decrypting and verifying if password matches
                if (UserClass.VerifyMd5Hash( password, pwd))
                {
                    //starting sessions
                    var status = dbList.Where(z => z.UserName.ToLower() == userID.ToLower()).Select(x => x.Status).FirstOrDefault();
                    if ( status == "Active")
                    {
                        HttpContext.Session.SetString("LoggedIn", "True");
                        HttpContext.Session.SetString("Username", userID);
                        HttpContext.Session.SetString("UserFailedReason", "None");

                        return RedirectToAction("Dashboard");
                    }

                    else
                    {
                        HttpContext.Session.SetString("LoggedIn", "False");
                        HttpContext.Session.SetString("UserFailedReason", "Suspended");

                        return RedirectToAction("Login");
                    }

                }
                else
                {
                    HttpContext.Session.SetString("LoggedIn", "False");
                    HttpContext.Session.SetString("UserFailedReason", "Incorrect");

                    return RedirectToAction("Login");

                }
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", new { controllerName = controller, actionName = action });

            }
        }


        public IActionResult Tracker()
        {
            try
            {
                string username = null;

                if (HttpContext.Session.GetString("LoggedIn") != null)
                {
                    username = HttpContext.Session.GetString("Username");
                }
                else

                    RedirectToAction("Index");
                var TrackerList = db.ProgressTracker.ToList();

                var RecordedWeeks = TrackerList.Where(z => z.UserName.ToLower() == username.ToLower()).Count();
                string[] weeks = new string[RecordedWeeks];

                for (int i = 0; i < RecordedWeeks; i++)
                {
                    weeks[i] = "Month " + (i + 1);
                }


                //change username for below when logged in
                var WeeklyBMI = TrackerList.Where(z => z.UserName.ToLower() == username.ToLower()).OrderBy(k => k.Month).Select(x => x.Bmi).ToArray();

                var WeeklyW2H = TrackerList.Where(z => z.UserName.ToLower() == username.ToLower()).OrderBy(k => k.Month).Select(x => x.Wc).ToArray();

                ViewBag.WeeklyW2Hs = WeeklyW2H;
                ViewBag.WeeklyBMIs = WeeklyBMI;
                ViewBag.weeks = weeks;

                return View();
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", new { controllerName = controller, actionName = action });

            }
        }
        

        public void SetActivities( string ActivitiesList)
        {

            string[] ActSet = ActivitiesList.Split(',');

            for(int i = 0; i < ActSet.Count(); i++)
            {
                DailyActivityTable AddActivity = new DailyActivityTable();
                string[] Act = ActSet[i].Split('=');
                AddActivity.RecordId = Guid.NewGuid();
                AddActivity.ActivityId = new Guid(Act[0]);
                AddActivity.Username =HttpContext.Session.GetString("Username");
                AddActivity.Duration = Act[1];
                AddActivity.RecordedDate = DateTime.Today;
                AddActivity.ActivityName = db.ExercisesListTable.Where(x => x.ExerciseId == new Guid(Act[0])).Select(c => c.ExerciseList).FirstOrDefault();

                db.Add(AddActivity);



                db.SaveChanges();


            }



        }

        public IActionResult _AddFoodPartial()
        {
            return View();
        }

        public IActionResult _NewUserPartial()
        {
            return View();
        }

        public IActionResult _ActivityModal()
        {
            return View();
        }


        public IActionResult _UpdateBMI()
        {
            string username = null;

            if (HttpContext.Session.GetString("LoggedIn") != null)
            {
                username = HttpContext.Session.GetString("Username");
            }
            else
                return RedirectToAction("Index");

            return View();


        }


        public IActionResult UpdateBMI(string WeightMetric, double weight, double waist, string HeightMetric, double heightMeter = 0, double heightFeet = 0, double heightInches = 0)
        {
            try
            {
                string username = null;

                if (HttpContext.Session.GetString("LoggedIn") != null)
                {
                    username = HttpContext.Session.GetString("Username");
                }
                else

                    return RedirectToAction("Index");
                double height;
                double UserWeight;
                if (HeightMetric == "Feet")
                {
                    height = heightFeet * 12;
                    height += heightInches;
                    height *= 0.025;

                }
                else
                    height = heightMeter;

                if (WeightMetric == "kg")
                {
                    UserWeight = weight;
                }
                else
                {
                    UserWeight = weight * 0.45;

                }

                //Saving the latest Weight data to Tracker table
                var TrackerList = db.ProgressTracker.ToList();
                var latestMonth = TrackerList.Where(z => z.UserName.ToLower() == username.ToLower()).Max(x => x.Month); //getting the latest week updated
                ProgressTracker FirstPRogress = new ProgressTracker();
                double BMI = weight / (height * height);
                FirstPRogress.TrackerId = Guid.NewGuid();
                FirstPRogress.UserName = username;
                FirstPRogress.Wc = waist;
                FirstPRogress.Bmi = BMI;
                FirstPRogress.MonthlyHeight = height;
                FirstPRogress.MonthlyWeight = UserWeight;
                FirstPRogress.DateEntered = DateTime.Today;
                FirstPRogress.Month = latestMonth + 1;
                db.Add(FirstPRogress);
                //end of saving to table Tracker

                var dietUser = db.DietUsers.Where(x => x.UserName.ToLower() == username.ToLower()).FirstOrDefault();

                float age = (float)Convert.ToDouble(dietUser.UserAge);

                int WCRange;
                if (dietUser.UserGender == "Male")
                {

                    if (waist < 94)
                        WCRange = 1;
                    else if (waist < 102)
                        WCRange = 2;
                    else
                        WCRange = 3;

                }
                else
                {
                    if (waist < 80)
                        WCRange = 1;
                    else if (waist < 88)
                        WCRange = 2;
                    else
                        WCRange = 3;
                }

                dietUser.UserWeightCategory = WeightClassification.returnClassification((float)BMI, WCRange);
                db.Update(dietUser);
                db.SaveChanges();

                return RedirectToAction("Dashboard");

            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", new { controllerName = controller, actionName = action });

            }

        }

        [HttpPost]
        public IActionResult _ViewAccountPartialView(string id)
        {
            try
            {
                if (id == null)
                {
                    id = HttpContext.Session.GetString("Username");
                }

                var dietUsers = db.DietUsers.Find(id);
                if (dietUsers == null)
                {
                    return NotFound();
                }
                UserClass getUser = new UserClass(id);
                return View(dietUsers);
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", new { controllerName = controller, actionName = action });

            }
        }

        public IActionResult Signout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");


                
        }

        public IActionResult Index()
        {
            //var k = db.DietUsers.ToList();
            //var s = k.Where(z => z.UserName == "Test").FirstOrDefault();

            //var sa = s;

            //string _Path = Path.Combine(Environment.CurrentDirectory, "Data", "exercises.txt");
            //ExercisesListTable ex = new ExercisesListTable();

            //System.IO.StreamReader file = new System.IO.StreamReader(_Path);
            //string line; int counter = 0;
            //string prev = ""; bool start = true;
            //while ((line = file.ReadLine()) != null)
            //{

            //    if (counter == 0) {
            //        counter++;
            //            ex.ExerciseList = line;
                  
            //        continue;


            //    }
            //    else if(counter == 1) {
            //        ex.Lb130 = Int32.Parse(line);
            //        counter++;
            //        continue;
            //    }

            //    else if (counter == 2)
            //    {
            //        ex.Lb155 = Int32.Parse(line);
            //        counter++;
            //        continue;
            //    }
            //    else if (counter == 3)
            //    {
            //        ex.Lb180 = Int32.Parse(line);
            //        counter++;
            //        continue;
            //    }
            //    else if (counter == 4)
            //    {
            //        ex.Lb205 = Int32.Parse(line);
            //        counter++;
            //        continue;
            //    }
            //    else
            //    {
            //        db.Add(ex);
            //        db.SaveChanges();
            //        counter = 1;
            //        ex = new ExercisesListTable();
            //        ex.ExerciseId = Guid.NewGuid();
            //        ex.ExerciseList = line;


            //    }
            //}



            if (HttpContext.Session.GetString("LoggedIn") != null)
            {
                ViewBag.Logged = HttpContext.Session.GetString("LoggedIn");
                ViewBag.Username = HttpContext.Session.GetString("Username");
            }
            else
            {
                ViewBag.Logged = "False";
            }

            return View();
        }




        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "ResetPassword")
        {

            var fromEmail = new MailAddress("eathealthy.website@gmail.com", "Eat Healthy");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "EatHealthy2k18"; 
            string subject = "";
            string body = "";
            if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";

                body = "<br/><br/> Please click on the link below to reset your Password <br/><br/>" +
                   "<a href='https://localhost:" + Request.Host.Port.ToString() + "/User/ResetPassword?id=" + activationCode + "' >Reset Password link </a>";

            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }


        public IActionResult ForgottenPassword()
        {
            ViewBag.SuccessMessage = "";
            ViewBag.FailedMessage = "";
            return View();
        }

        [HttpPost]
        public IActionResult ForgottenPassword(string EmailID)
        {
            HtmlString successHtml = null;
            HtmlString errHtml = null;
            try
            {
                var dbList = db.DietUsers.ToList();
                var EmailExist = dbList.Where(z => z.UserEmail == EmailID).FirstOrDefault();
                if (EmailExist != null)
                {

                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(EmailExist.UserEmail, resetCode, "ResetPassword");
                    EmailExist.ResetPasswordCode = resetCode;
                    string errString = "<div class='alert alert-success'>Email has been sent successfully </div>";
                    successHtml = new HtmlString(errString);

                    db.SaveChanges();


                }
                else
                {
                    string errString = "<div class='alert alert-danger'><strong> Error </strong> No account found under the email provided  </div>";
                    errHtml = new HtmlString(errString);

                }
                ViewBag.SuccessMessage = successHtml;
                ViewBag.FailedMessage = errHtml;

                return View();
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", new { controllerName = controller, actionName = action });

            }
        }
        public IActionResult ResetPassword(string id)
        {
            
            var dbList = db.DietUsers.ToList();
            var user = dbList.Where(z => z.ResetPasswordCode == id).FirstOrDefault();
            if (user != null)
            {
                ResetPasswordModel model = new ResetPasswordModel();
                model.ResetCode = id;
                return View(model);
            }
            else
                return NotFound();
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordModel model)
        {
            try
            {
                var message = "";
                if (ModelState.IsValid)
                {
                    var dbList = db.DietUsers.ToList();
                    var user = dbList.Where(z => z.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)

                    {
                                    MD5 md5Hash = MD5.Create();


                        user.UserPassword = UserClass.GetMd5Hash(md5Hash, model.NewPassword);

                        user.ResetPasswordCode = "";
                        db.SaveChanges();
                        message = "Password updated Successfully";
                    }
                    else
                        message = "Something invalid";

                }
                ViewBag.Message = message;
                return View(model);
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", new { controllerName = controller, actionName = action });

            }
        }


        public List<double> SetDailyConsumption( string ConsumptionList)
        {
            string[] ConsumptionSet = ConsumptionList.Split(',');

            for (int i = 0; i < ConsumptionSet.Count(); i++)
            {
                UserDailyConsumption AddRecord = new UserDailyConsumption();
                string[] Act = ConsumptionSet[i].Split('=');
                AddRecord.RecordId = Guid.NewGuid();
                AddRecord.ConsumptionId = int.Parse( Act[0]);
                AddRecord.Username = HttpContext.Session.GetString("Username");
                AddRecord.RecordedDate = DateTime.Today;
                AddRecord.ConsumptionQuantity = int.Parse(Act[1]);
                db.Add(AddRecord);

                db.SaveChanges();
            }
            UserClass ThisUser = new UserClass(HttpContext.Session.GetString("Username"));
       
            return ThisUser.CalConsumption();
        }

        public IActionResult Dashboard()
        {
          

            try
            {
                string username = null;

                if (HttpContext.Session.GetString("LoggedIn") != null)
                {
                    username = HttpContext.Session.GetString("Username");
                   
                }
                else

                    return RedirectToAction("Index");

               

                DashboardFunctions loadUser = new DashboardFunctions(username);
                UserClass thisUser = new UserClass(username);
                ViewBag.TodayAct = loadUser.GetDailyActivityList();
                var TrackerList = db.ProgressTracker.ToList();
                var exerciseList = db.ExercisesListTable.ToList();
                var existFood = db.FoodData.OrderBy(z => z.Id).ToList();
                DateTime dt = DateTime.Now;
                var latestMonth = TrackerList.Where(z => z.UserName.ToLower() == username.ToLower()).Max(x => x.Month); //latest week since update

                //get Current date in the format specified
                ViewBag.date = dt.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("en-US"));

                ViewBag.TimeExceeded = loadUser.TimeOverdue(dt, latestMonth);
                var RecordedWeeks = TrackerList.Where(z => z.UserName.ToLower() == username.ToLower()).Count();
                string[] weeks = new string[RecordedWeeks];

                for (int i = 0; i < RecordedWeeks; i++)
                {
                    weeks[i] = "Month " + (i + 1);
                }

                var WeeklyBMI = TrackerList.Where(z => z.UserName.ToLower() == username.ToLower()).OrderBy(k => k.Month).Select(x => x.Bmi).ToArray();
                var exercisesArray = exerciseList.OrderBy(f => f.ExerciseList).Select(s => s.ExerciseList.Substring(0, s.ExerciseList.Length - 1)).ToArray();
                var exercisesIDArray = exerciseList.OrderBy(f => f.ExerciseList).Select(s => s.ExerciseId).ToArray();
                var FoodsDBArray = existFood.Select(z => z.Food).ToArray();
                var FoodsIDArray = existFood.Select(z => z.Id).ToArray();
                var proteins = existFood.Select(z => Math.Round( (double)z.Protein,2)).ToArray();
                var Cal = existFood.Select(z => Math.Round((double)z.Calorie,2)).ToArray();
                var Carb = existFood.Select(z => Math.Round((double)z.Carbs, 2)).ToArray();
                var Fat = existFood.Select(z => Math.Round((double)z.Fat, 2)).ToArray();
                string [] macroString = new string[Cal.Length];
                for (int k = 0; k<Cal.Length; k++)
                {
                    macroString[k] = "   |Calorie:"+Cal[k] +"  |Protein:"+proteins[k] +"  |Carb:"+ Carb[k] +"  |Fats:"+Fat[k]+"|"; 
                }
                ViewBag.MacroString = macroString;


                var WeeklyW2H = TrackerList.Where(z => z.UserName.ToLower() == username.ToLower()).OrderBy(k => k.Month).Select(x => x.Wc).ToArray();
                ViewBag.BMI = Math.Round( TrackerList.Where(z => z.UserName.ToLower() == username.ToLower() && z.Month == latestMonth).Select(x => x.Bmi).FirstOrDefault(), 2);
                ViewBag.Weight = TrackerList.Where(z => z.UserName.ToLower() == username.ToLower() && z.Month == latestMonth).Select(x => x.MonthlyWeight).FirstOrDefault();
                var newValues = exercisesArray.Select(x => x.Substring(0, x.Length - 1)).ToArray();

                ViewBag.Consumption = thisUser.CalConsumption();

                ViewBag.ExerciseIDArr = exercisesIDArray;
                ViewBag.ExerciseArr = exercisesArray;

                ViewBag.FoodsArr = FoodsDBArray;
                ViewBag.FoodsIDArr = FoodsIDArray;

                ViewBag.WeeklyW2Hs = WeeklyW2H;
                ViewBag.WeeklyBMIs = WeeklyBMI;
                ViewBag.weeks = weeks;

                ViewBag.username = username;

                var dietuser = db.DietUsers.Find(username);

                ViewBag.NewUser = dietuser.NewUser; //display model if its a new user

                ViewBag.Macros = db.UserMacros.Where(z => z.Username.ToLower() == username.ToLower()).FirstOrDefault();
                ViewBag.ChoosenAllegies = loadUser.GetAllergiesList();

              
                ViewBag.ChoosenPreferences = loadUser.GetPreferencesList();

                return View();
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", new { controllerName = controller, actionName = action });

            }
        }


        //public ActionResult APIResult()
        //{
        //    ViewBag.Script = new CoreDietPlan.Models.APICall().PostRequest("https://trackapi.nutritionix.com/v2/natural/nutrients");
        //    return View();
        //}
        public List<Meal> AprioriResult()
        {
            string username = "Adrien123";

            //check if user is logged in
            //if (HttpContext.Session.GetString("LoggedIn") != null)
            //{
            //    //retrieve user details from respective database tables
            //    username = HttpContext.Session.GetString("Username");
            //}

            var BreakFastDietplan = db.DietDb.Where(x => x.MealTime.Contains("Breakfast") && x.Username==username).Select(x => x.PrefId.ToString()).ToArray();
            var LunchDietplan = db.DietDb.Where(x => x.MealTime.Contains("Lunch") && x.Username == username).Select(x => x.PrefId.ToString()).ToArray();
            var DinnerDietplan = db.DietDb.Where(x => x.MealTime == "Dinner" && x.Username == username).Select(x => x.PrefId.ToString()).ToArray();
            //ViewBag.ScriptApriori = new Apriori().AprioriAI(BreakFastDietplan,LunchDietplan,DinnerDietplan);
            string[][] matches = new Apriori().AprioriAI(BreakFastDietplan, LunchDietplan, DinnerDietplan);
            var food = new HashSet<int> { };

            foreach (string[] i in matches)
            {
                foreach (string item in i)
                {
                    //food = db.DietDb.Where(x => x.prefID == Int32.Parse(item)).Select(x => x.FoodId).ToList();

                    food.Add(Int32.Parse(item));

                }

            }
            List<string> pref = db.FoodDb.Where(x => food.Any(a => a == x.Id)).Select(x => x.Food).ToList();
            //int[] newstr = new HashSet<int>(food).ToArray();
            //foreach (var f in pref)
            //{
            //    System.Diagnostics.Debug.WriteLine(f);

            //}
            List<Meal> UsrPref = db.FoodData.Where(x => pref.Any(a => a == x.Food)).Select(x => new Meal { food = x.Food, protein = x.Protein.ToString(), carb = x.Carbs.ToString(), fat = x.Fat.ToString(), calorie = x.Calorie.ToString(), fiber = x.Fiber.ToString(), img = x.FoodImg, serving = x.Serving, servingQty = x.ServingQty.ToString() }).ToList();
            //foreach (Meal f in UsrPref)
            //{
            //    System.Diagnostics.Debug.WriteLine(f.food+"\n"+f.calorie);

            //}
            //ViewBag.ScriptApriori = UsrPref;
            return UsrPref;
        }

        public ActionResult DietPlanner(List<Meal> s)
        {

            //ViewBag.UserName = usr;

            ViewBag.ScriptD = new DisplayDiet().CreateDiet();
            ViewBag.Meals = new DisplayDiet().GetMeals();
            List<Meal>meals= new DisplayDiet().GetMeals();
            ViewBag.Users = new DisplayDiet().GetUsr();
            ViewBag.Macros = new DisplayDiet().getMacros();
            //ViewBag.Div = new HTMLhelper().FoodDiv();
            //ViewBag.ScriptApriori = AprioriResult();
            //ViewBag.Selected = s;
            return View();
        }
        public List<Meal> Apriori()
        {
            List<Meal>meals= AprioriResult();
            List<Meal> positive = new DisplayDiet().GetMeals();
            positive = positive.Where(x => meals.Any(y => x.food.Contains(y.food, StringComparison.OrdinalIgnoreCase))).ToList();
            return positive;
        }
        public ActionResult MLTest()
        {
            try
            {
                var dbList = db.DietUsers.ToList();
                var prefList = db.Preferences.ToList();
                var allergyList = db.Allergies.ToList();
                var progressTrackerList = db.ProgressTracker.ToList();
                string username = null;
                List<User> users = new List<User> { };
                //check if user is logged in
                if(HttpContext.Session.GetString("LoggedIn") != null)
                {
                    //retrieve user details from respective database tables
                    username = HttpContext.Session.GetString("Username");
                    string bmi = dbList.Where(z => z.UserName.ToLower() == username.ToLower()).Select(x => x.UserWeightCategory).FirstOrDefault();
                    bool? veg = dbList.Where(z => z.UserName == username).Select(x => x.IsVeg).FirstOrDefault();
                    //string pref = prefList.Where(z => z.Username == username).Select(x => x.PreferencesList).FirstOrDefault();
                    string allergy = allergyList.Where(z => z.UserName == username).Select(x => x.AllergiesList).FirstOrDefault();
                    string activity = progressTrackerList.Where(z => z.UserName.ToLower() == username.ToLower()).Select(x => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.ActivityLevel.ToLower())).FirstOrDefault();
                    double? tdee = progressTrackerList.Where(z => z.UserName.ToLower() == username.ToLower()).Select(x => x.Tdee).FirstOrDefault();

                    //string bmi = "obese";
                    // bool? veg = false;
                    // string allergy = "shellfish";
                    // string activity = "Sedentary";
                    // double? tdee = 3000;


                    //store details in list
                    //users.Add(new User { user = username, bmi = bmi.ToLower(),veg=veg,preference=pref,allergy=allergy });
                    users.Add(new User { user = username, bmi = bmi.ToLower(), activity = activity, tdee = tdee, allergy = allergy });
                    //send the list to the machine learning algorithms and store the returned results in a viewbag
                    ViewBag.ScriptML = new FoodClassification(users);


                }
                else
                {
                    return RedirectToAction("Login");
                }

                //return View();
                return RedirectToAction("DietPlanner");
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", new { controllerName = controller, actionName = action });

            }
        }

        public List<Meal> getFoods(string food, float protein, float carb, float fat, float cal, string serving, string image,string mealTime,int prefID)
        {

            //array.Add(new Meal { food = food, nprotein = protein, ncarb = carb, nfat = fat, ncalorie = cal, serving = serving, img = image, meal_time = mealTime,PrefID=prefID });
            //remove = array;
            //DietPlanner(array);

            //List<Meal> list = HttpContext.Session.GetObjectFromJson<List<Meal>>("FoodList") ?? new List<Meal>();
            List<Meal> getlist = HttpContext.Session.GetCart();
            List<Meal> list = new List<Meal> { };
            list.Add(new Meal { food = food, nprotein = protein, ncarb = carb, nfat = fat, ncalorie = cal, serving = serving, img = image, meal_time = mealTime });
            getlist.Add(new Meal { food = food, nprotein = protein, ncarb = carb, nfat = fat, ncalorie = cal, serving = serving, img = image, meal_time = mealTime });
            //HttpContext.Session.SetObjectAsJson("FoodList", list);
            HttpContext.Session.SaveCart(getlist);


            //List<Meal> combined = GetDiet().ToList();
            //return array;

            return list;
        }
        public List<Meal> LoadFoods()
        {
            List<Meal> GetList = HttpContext.Session.GetCart();

            //array.Clear();
            //return array;
            return GetList;
        }
        public List<Meal> removeFoods(string food)
        {

            //var foods = array.Where(a => a.food.Equals(food));
            //array.Where(x => x.food.Equals(food)).Select(i => new { i.nprotein, i.ncarb, i.ncalorie, i.nfat, i.serving, i.img });
            // array.RemoveAll(x => x is Meal && (x as Meal).food.Equals(food));
            //array = array.Where(x => x.food!=food).ToList();

            //return array;

            List<Meal> GetList = HttpContext.Session.GetCart().ToList();
            GetList = GetList.Where(x=>x.food!=food).ToList();
            HttpContext.Session.SaveCart(GetList);

            return GetList;

        }
        public ActionResult SaveDiet(string food, float protein, float carb, float fat, float cal, string serving, string image,int pref_ID)
        {

            DateTime dateTime = DateTime.UtcNow.Date;
            try
            {

                string username = null;

                //check if user is logged in
                if (HttpContext.Session.GetString("LoggedIn") != null)
                {
                    //retrieve user details from respective database tables
                    username = HttpContext.Session.GetString("Username");

                    foreach (var foods in HttpContext.Session.GetCart())
                    {
                        using (var context = new DietPlanDBContext())
                        {
                            //var diet = new DietDb { Username = username, Date = dateTime, MealTime = foods.meal_time, Meals = foods.food };
                            //var nutrients = new NutritionalValue { Date = dateTime, FoodImg = foods.img, Protein = foods.nprotein, Carbs = foods.ncarb, Fat = foods.nfat, Calorie = foods.ncalorie };
                            //context.DietDb.Add(diet);
                            //context.NutritionalValue.Add(nutrients);
                            //context.SaveChanges();

                            var diet = new DietDb { Username = username, Date = dateTime, MealTime = foods.meal_time, Meals = foods.food ,PrefId=pref_ID};

                            context.DietDb.Add(diet);
                            context.SaveChanges();

                            var nutrients = new NutritionalValue { Food = foods.food, FoodId = diet.FoodId, Date = dateTime, FoodImg = foods.img, Protein = foods.nprotein, Carbs = foods.ncarb, Fat = foods.nfat, Calorie = foods.ncalorie, MealTime = foods.meal_time };

                            context.NutritionalValue.Add(nutrients);
                            context.SaveChanges();
                        }
                    }

                }
                else
                {
                    return RedirectToAction("Login");
                }

                return View();

            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", new { controllerName = controller, actionName = action });

            }


        }
        public List<Meal> Generate(string food, float protein, float carb, float fat, float cal, string serving, string image, string mealTime)
        {
            List<Meal>filter = new DisplayDiet().GetMeals().Where(x => x.ncalorie < 100).Take(4).ToList();
            //Meal Foo = positive.Where(x => x.ncalorie < 100).FirstOrDefault();
            
            return filter;
        }
        public List<Meal> GetDiet()
        {
            string username = null;
            if (HttpContext.Session.GetString("LoggedIn") != null)
            {
                username = HttpContext.Session.GetString("Username");

            }
            //&& x.Date == db.DietDb.Max(y => y.Date)
            var id = db.DietDb.Where(x => x.Username == username).OrderByDescending(x => x.Date).Select(x => new { x.FoodId, x.Meals, x.MealTime }).ToList();
            
            List<Meal> diet = db.NutritionalValue.Where(x => id.Any(a => a.FoodId == x.FoodId)).Select(x => new Meal { food = x.Food, meal_time = x.MealTime, img = x.FoodImg, protein = x.Protein.ToString(), carb = x.Carbs.ToString(), fat = x.Fat.ToString(), calorie = x.Calorie.ToString(), serving = x.Serving.ToString() }).ToList();
        

            return diet;
        }
        //public Dictionary<string, List<Meal>> proposingFood(double AllocatedProtein, double AllocatedCarb, double AllocatedFat, double AllocatedCalorie)
        //{

        //    var newProposed = new DisplayDiet();
        //    return newProposed.ProposedDiet(AllocatedProtein, AllocatedCarb, AllocatedFat, AllocatedCalorie);
        //}
        //public ActionResult proposed(double AllocatedProtein, double AllocatedCarb, double AllocatedFat, double AllocatedCalorie)
        //{
        //    AllocatedProtein = 40;
        //    AllocatedCarb = 100;
        //    AllocatedFat = 30;
        //    AllocatedCalorie = 1000;
        //    var newProposed = new DisplayDiet();
        //    ViewBag.propose = newProposed.ProposedDiet(AllocatedProtein, AllocatedCarb, AllocatedFat, AllocatedCalorie);

        //    return View();
        //}
        public Dictionary<string, List<Meal>> returnPropose()
        {
            var m = new Dictionary<string, List<Meal>> { };
            m = new DisplayDiet().getDict();
            return m;
        }
        public List<Meal> Clear()
        {
            List<Meal> currentMeals = HttpContext.Session.GetCart();
            List<Meal> current = new List<Meal> { };
            current = currentMeals.ToList();
            current.Clear();
            HttpContext.Session.SaveCart(current);


            return current;
        }
        public ActionResult DataSet()
        {
            ViewBag.Dataset = new createDataSet();
            return View();
        }
        public void UpdateAllergies(string Allergies)
        {
            string username = HttpContext.Session.GetString("Username");
            DashboardFunctions UpdateUser = new DashboardFunctions(username);

            if(UpdateUser.UpdateAllergies(Allergies)== false)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                RedirectToAction("Error", new { controllerName = controller, actionName = action });
            }

        }
        public void UpdatePreferences(string Preferences)
        {
           
            string username = HttpContext.Session.GetString("Username");
            DashboardFunctions UpdateUser = new DashboardFunctions(username);

            if (UpdateUser.UpdatePreferences(Preferences) == false)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                RedirectToAction("Error", new { controllerName = controller, actionName = action });
            }

        }

        public IActionResult Error(string controllerName = "", string actionName = "")
        {

            if (controllerName != "" && actionName != "")
            {
                ViewBag.actionName = actionName;
                ViewBag.controllerName = controllerName;
                ViewBag.set = "True";
            }
            else
            {
                ViewBag.actionName = "Login";
                ViewBag.controllerName = "Admin";
                ViewBag.set = "false";

            }
            return View();

        }



    }
}
