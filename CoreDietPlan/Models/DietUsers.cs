using System;
using System.Collections.Generic;

namespace CoreDietPlan.Models
{
    public partial class DietUsers
    {
        public string UserName { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public int? UserAge { get; set; }
        public string UserEmail { get; set; }
        public string UserGender { get; set; }
        public string UserWeightCategory { get; set; }
        public string ResetPasswordCode { get; set; }
        public bool? NewUser { get; set; }
        public bool? IsVeg { get; set; }
        public bool? SetPicture { get; set; }
        public string UserPassword { get; set; }
        public string Status { get; set; }
    }
}
