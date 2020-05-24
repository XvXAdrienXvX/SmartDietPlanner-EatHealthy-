using Microsoft.ML.Runtime.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDietPlan.Models
{
    public class DataSet
    {
        [Column(ordinal: "0")]
        public string FGroup { get; set; }
        [Column(ordinal: "1")]
        public float protein { get; set; }
        [Column(ordinal: "2")]
        public float fat { get; set; }
        [Column(ordinal: "3")]
        public float carb { get; set; }
        [Column(ordinal: "4")]
        public float calorie { get; set; }
        [Column(ordinal: "5")]
        public float fiber { get; set; }
        //[Column(ordinal: "6")]
        //public string decision { get; set; }

        //[Column(ordinal: "0")]
        //public string item_name { get; set; }
        //[Column(ordinal: "1")]
        //public float nf_protein { get; set; }
        //[Column(ordinal: "2")]
        //public float nf_total_fat { get; set; }
        //[Column(ordinal: "3")]
        //public float nf_total_carbohydrate { get; set; }
        //[Column(ordinal: "4")]
        //public float nf_calories { get; set; }
        //[Column(ordinal: "5")]
        //public float nf_dietary_fiber { get; set; }
    }
}
