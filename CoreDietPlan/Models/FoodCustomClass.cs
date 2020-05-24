using Microsoft.ML.Runtime.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDietPlan.Models
{
    public class FoodCustomClass
    {
        [Column(ordinal: "0")]
        public string Food;

        //[Column(ordinal: "1")]
        //public string Serving;

        [Column(ordinal: "1")]
        public float Total_Protein;

        [Column(ordinal: "2")]
        public float Total_Carb;

        [Column(ordinal: "3")]
        public float Total_Fat;

        [Column(ordinal: "4")]
        public float Num_Calorie;

        [Column(ordinal: "5")]
        public float Fiber;

        [Column(ordinal: "6", name: "Label")]
        public string Class;

        [Column(ordinal: "7")]
        public string img;

        [Column(ordinal: "8")]
        public int fgroup;

        [Column(ordinal: "9")]
        public int ID;

        [Column(ordinal: "10")]
        public int ServingQty;

        [Column(ordinal: "11")]
        public string Serving;

    }
    public class ClassPrediction
    {
        [ColumnName("PredictedLabel")]
        public string Predicted;

        //[ColumnName("Probability")]
        //public float Probability { get; set; }

        //[ColumnName("Score")]
        //public float[] Score { get; set; }
    }
}
