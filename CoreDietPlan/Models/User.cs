using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDietPlan.Models
{
    public class User
    {
        public string user { get; set; }
        public string bmi { get; set; }
        public string preference { get; set; }
        public string allergy { get; set; }
        public float protein { get; set; }
        public float carbs { get; set; }
        public float fat { get; set; }
        public double? cal { get; set; }
        public float weight { get; set; }
        public float weightKg = 2.205f;
        public double? tdee { get; set; }
        public string activity { get; set; }
        public bool? veg { get; set; }
        //List<User> users = new List<User> { };
        //public List<User> SetUser(string username,string bmi)
        //{

        //    users.Add(new User { user = username,bmi=bmi.ToLower() });
        //    //foreach (User u in users)
        //    //{
        //    //    System.Diagnostics.Debug.WriteLine("User:" + u.user + "\n" + "BMI:" + u.bmi);
        //    //}

        //    return users;
        //}

    }
    
}
