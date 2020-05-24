using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections;
using System.Data;
using CsvHelper;
using System.IO;
using Accord;
using System.Text;
using System.Globalization;

namespace CoreDietPlan.Models
{
    public class FullNutrient
    {
        [JsonProperty("attr_id")]
        public int attr_id { get; set; }
        [JsonProperty("value")]
        public double value { get; set; }
    }

    public class Metadata
    {
        [JsonProperty("is_raw_food")]
        public bool is_raw_food { get; set; }
    }

    public class Tags
    {
        [JsonProperty("item")]
        public string item { get; set; }
        [JsonProperty("measure")]
        public string measure { get; set; }
        [JsonProperty("quantity")]
        public string quantity { get; set; }
        [JsonProperty("food_group")]
        public int food_group { get; set; }
        [JsonProperty("tag_id")]
        public int tag_id { get; set; }
    }

    public class AltMeasure
    {
        [JsonProperty("serving_weight")]
        public double serving_weight { get; set; }
        [JsonProperty("measure")]
        public string measure { get; set; }
        [JsonProperty("seq")]
        public int? seq { get; set; }
        [JsonProperty("qty")]
        public double qty { get; set; }
    }

    public class Photo
    {
        [JsonProperty("thumb")]
        public string thumb { get; set; }
        [JsonProperty("highres")]
        public string highres { get; set; }
        [JsonProperty("is_user_uploaded")]
        public bool is_user_uploaded { get; set; }
    }

    public class Food
    {
        [JsonProperty("food_name")]
        public string food_name { get; set; }
        [JsonProperty("brand_name")]
        public object brand_name { get; set; }
        [JsonProperty("serving_qty")]
        public int serving_qty { get; set; }
        [JsonProperty("serving_unit")]
        public string serving_unit { get; set; }
        [JsonProperty("serving_weight_grams")]
        public string serving_weight_grams { get; set; }
        [JsonProperty("nf_calories")]
        public string nf_calories { get; set; }
        [JsonProperty("nf_total_fat")]
        public string nf_total_fat { get; set; }
        [JsonProperty("nf_saturated_fat")]
        public string nf_saturated_fat { get; set; }
        [JsonProperty("nf_cholesterol")]
        public string nf_cholesterol { get; set; }
        [JsonProperty("nf_sodium")]
        public string nf_sodium { get; set; }
        [JsonProperty("nf_total_carbohydrate")]
        public string nf_total_carbohydrate { get; set; }
        [JsonProperty("nf_dietary_fiber")]
        public string nf_dietary_fiber { get; set; }
        [JsonProperty("nf_sugars")]
        public string nf_sugars { get; set; }
        [JsonProperty("nf_protein")]
        public string nf_protein { get; set; }
        [JsonProperty("nf_potassium")]
        public string nf_potassium { get; set; }
        [JsonProperty("nf_p")]
        public double nf_p { get; set; }
        [JsonProperty("full_nutrients")]
        public List<FullNutrient> full_nutrients { get; set; }
        [JsonProperty("nix_brand_name")]
        public object nix_brand_name { get; set; }
        [JsonProperty("nix_brand_id")]
        public object nix_brand_id { get; set; }
        [JsonProperty("nix_item_name")]
        public object nix_item_name { get; set; }
        [JsonProperty("nix_item_id")]
        public object nix_item_id { get; set; }
        [JsonProperty("upc")]
        public object upc { get; set; }
        [JsonProperty("consumed_at")]
        public DateTime consumed_at { get; set; }
        [JsonProperty("metadata")]
        public Metadata metadata { get; set; }
        [JsonProperty("source")]
        public int source { get; set; }
        [JsonProperty("ndb_no")]
        public int ndb_no { get; set; }
        [JsonProperty("tags")]
        public Tags tags { get; set; }
        [JsonProperty("alt_measures")]
        public List<AltMeasure> alt_measures { get; set; }
        [JsonProperty("lat")]
        public object lat { get; set; }
        [JsonProperty("lng")]
        public object lng { get; set; }
        [JsonProperty("meal_type")]
        public int meal_type { get; set; }
        [JsonProperty("photo")]
        public Photo photo { get; set; }
        [JsonProperty("sub_recipe")]
        public object sub_recipe { get; set; }
    }

    public class RootObject
    {
        [JsonProperty("foods")]
        public List<Food> foods { get; set; }
    }
    public class nutrient
    {
        public string food { get; set; }
        public string protein { get; set; }
        public string carb { get; set; }
        public string fat { get; set; }
        public string calorie { get; set; }
        public string serving { get; set; }
        public Photo img { get; set; }
        public int fgroup { get; set; }
    }
    public class APICall
    {
        public string path = Path.Combine(Environment.CurrentDirectory, "Data", "food.csv");

        Dictionary<int,string> meals = new Dictionary<int,string>();
        List<nutrient> nutrients = new List<nutrient> { };
        DietPlanDBContext context=new DietPlanDBContext();
        public Dictionary<int,string> getmeals()
        {
            var csv = new CsvReader(File.OpenText(path));
            var myCustomObjects = csv.GetRecords<foodCSV>();


            foreach (var record in myCustomObjects)
            {
                meals.Add(record.FoodID, record.Food);
            }

            //var f = context.FoodDb.ToList();
            //foreach (var food in f)
            //{
            //    //if(food.Serving==null)
            //    //{
            //    //    food.Serving = "";
            //    //}
            //    string foods = food.ServingQty.ToString() + " " + food.Serving + " " + food.Food;
            //    meals.Add(food.Id, foods);
            //}

            return meals;
        }
        public DataTable PostRequest(string url)
        {
             getmeals();
            //Apriori a = new Apriori();
            //string[][]result=a.AprioriAI();
            
            var queries = new List<KeyValuePair<string, string>>();
            StringBuilder sb = new StringBuilder();
            string valueSep = " and ";
            foreach (var items in meals.Values)
            {
                sb.Append(items);
                sb.Append(valueSep);
    
            }
            string foods= sb.ToString(0, sb.Length - valueSep.Length); 
            queries.Add(new KeyValuePair<string, string>("query", foods));
       

            HttpContent q = new FormUrlEncodedContent(queries);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-app-id", "11bddcae");
                client.DefaultRequestHeaders.Add("x-app-key", "285fb98d493d0404238960830ecc5fb8");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.PostAsync(url, q).Result;
                var content = response.Content;

                RootObject list = JsonConvert.DeserializeObject<RootObject>(content.ReadAsStringAsync().Result);

                //foreach (Food item in list.foods)
                //{

                //    nutrients.Add(new nutrient { food=item.food_name,protein=item.nf_protein,carb=item.nf_total_carbohydrate,fat=item.nf_total_fat,calorie=item.nf_calories,serving=item.serving_unit,fgroup=item.tags.food_group,img=item.photo});
                //}
             
                DataTable dt = new DataTable();
                DataRow row;
                dt.Columns.Add("ID","Food", "Protein", "Carb", "Fat", "Calorie","Fiber","serving","servingQty","img","fgroup");
                
                //foreach (Food item in list.foods)
                //{
                //    row = dt.NewRow();

                //    row["Food"] = item.food_name;
                //    row["Protein"] = item.nf_protein;
                //    row["Carb"] = item.nf_total_carbohydrate;
                //    row["Fat"] = item.nf_total_fat;
                //    row["Calorie"] = item.nf_calories;
                //    row["Fiber"] = item.nf_dietary_fiber;
                //    row["serving"] = item.serving_unit;
                //    row["img"] = item.photo.thumb;
                //    row["fgroup"] = item.tags.food_group;
                //    dt.Rows.Add(row);
                //}
                foreach (var pair in meals.Keys.Zip(list.foods, Tuple.Create))
                {
                    row = dt.NewRow();

                    row["ID"] = pair.Item1;
                    row["Food"] = pair.Item2.food_name;
                    row["Protein"] = pair.Item2.nf_protein;
                    row["Carb"] = pair.Item2.nf_total_carbohydrate;
                    row["Fat"] = pair.Item2.nf_total_fat;
                    row["Calorie"] = pair.Item2.nf_calories;
                    row["Fiber"] = pair.Item2.nf_dietary_fiber;
                    row["serving"] = pair.Item2.serving_unit;
                    row["servingQty"] = pair.Item2.serving_qty;
                    row["img"] = pair.Item2.photo.thumb;
                    row["fgroup"] = pair.Item2.tags.food_group;
                    dt.Rows.Add(row);
                }
                //foreach (Food f in list.foods)
                //{
                    //bool isexists = context.FoodData.Any(x => x.Food == f.food_name);
                    //if (!isexists)
                    //{
                        //using (var context = new DietPlanDBContext())
                        //{
                        //    var values = new FoodData
                        //    {
                        //        Food = f.food_name,
                        //        FoodImg = f.photo.thumb,
                        //        Fgroup = f.tags.food_group,
                        //        Protein = float.Parse(f.nf_protein, CultureInfo.InvariantCulture.NumberFormat),
                        //        Carbs = float.Parse(f.nf_total_carbohydrate, CultureInfo.InvariantCulture.NumberFormat),
                        //        Fat = float.Parse(f.nf_total_fat, CultureInfo.InvariantCulture.NumberFormat),
                        //        Fiber = float.Parse(f.nf_dietary_fiber, CultureInfo.InvariantCulture.NumberFormat),
                        //        Calorie = float.Parse(f.nf_calories, CultureInfo.InvariantCulture.NumberFormat),
                        //        Serving = f.serving_unit,
                        //        ServingQty = f.serving_qty
                        //    };

                        //    context.FoodData.Add(values);

                        //    context.SaveChanges();

                        //}
                    //}
                //}
                //foreach (DataRow rows in dt.Rows)
                //{
                //    System.Diagnostics.Debug.WriteLine("Food: "+rows["ID"]+": "+rows["Food"].ToString());
                //}

                return dt;
            }
        }
       
    }
}