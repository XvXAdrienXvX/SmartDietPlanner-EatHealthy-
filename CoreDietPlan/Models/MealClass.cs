using Microsoft.ML.Runtime.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDietPlan.Models
{
    public class MealClass
    {
        //[Column("0")]
        //public string brand_name { get; set; }
        //[Column("1")]
        //public string item_name { get; set; }
        //[Column("2")]
        //public string brand_id { get; set; }
        //[Column("3")]
        //public string item_id { get; set; }
        //[Column("4")]
        //public string upc { get; set; }
        //[Column("5")]
        //public int item_type { get; set; }
        //[Column("6")]
        //public string item_description { get; set; }
        //[Column("7")]
        //public string nf_ingredient_statement { get; set; }
        //[Column("8")]
        //public float nf_calories { get; set; }
        //[Column("9")]
        //public int nf_calories_from_fat { get; set; }
        //[Column("10")]
        //public float nf_total_fat { get; set; }
        //[Column("11")]
        //public int nf_saturated_fat { get; set; }
        //[Column("12")]
        //public string nf_trans_fatty_acid { get; set; }
        //[Column("13")]
        //public int nf_polyunsaturated_fat { get; set; }
        //[Column("14")]
        //public int nf_monounsaturated_fat { get; set; }
        //[Column("15")]
        //public int nf_cholesterol { get; set; }
        //[Column("16")]
        //public int nf_sodium { get; set; }
        //[Column("17")]
        //public float nf_total_carbohydrate { get; set; }
        //[Column("18")]
        //public int nf_dietary_fiber { get; set; }
        //[Column("19")]
        //public int nf_sugars { get; set; }
        //[Column("20")]
        //public float nf_protein { get; set; }
        //[Column("21")]
        //public int nf_vitamin_a_dv { get; set; }
        //[Column("22")]
        //public int nf_vitamin_c_dv { get; set; }
        //[Column("23")]
        //public int nf_calcium_dv { get; set; }
        //[Column("24")]
        //public int nf_iron_dv { get; set; }
        //[Column("25")]
        //public string nf_servings_per_container { get; set; }
        //[Column("26")]
        //public int nf_serving_size_qty { get; set; }
        //[Column("27")]
        //public string nf_serving_size_unit { get; set; }
        //[Column("28")]
        //public int nf_serving_weight_grams { get; set; }
        //[Column("29")]
        //public DateTime updated_at { get; set; }

        [Column("0")]
        public string food { get; set; }

        [Column("1")]
        public float carb{ get; set; }

        [Column("2")]
        public float protein { get; set; }

        [Column("3")]
        public float fat { get; set; }

        [Column("4")]
        public float calorie { get; set; }

        [Column("5")]
        public string category;
    }
    public class ClusterPrediction
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedCategory;

        //[ColumnName("Score")]
        //public float[] Distances;
    }

}
