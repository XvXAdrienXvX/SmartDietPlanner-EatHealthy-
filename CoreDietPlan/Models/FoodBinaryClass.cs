using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.Runtime.Api;
namespace CoreDietPlan.Models
{
    public class FoodBinaryClass
    {

        [Column(ordinal: "0")]
        public string meal { get; set; }

        [Column(ordinal: "1")]
        public string Availability { get; set; }

        [Column(ordinal: "2")]
        public string Category { get; set; }

        [Column(ordinal: "3")]
        public string Carb { get; set; }

        [Column(ordinal: "4")]
        public string Protein { get; set; }

        [Column(ordinal: "5")]
        public string Fat { get; set; }

        [Column(ordinal: "6")]
        public string Calorie { get; set; }

        [Column(ordinal: "7")]
        public string Preference { get; set; }

        [Column(ordinal: "8")]
        public string outcome { get; set; }

        [Column(ordinal: "9", name: "Label")]
        public float decision { get; set; }

    }
    public class FPrediction
    {
        [ColumnName("PredictedLabel")]
        public float Prediction { get; set; }

        [ColumnName("Probability")]
        public float Probability { get; set; }

        [ColumnName("Score")]
        public float Score { get; set; }
    }
}
