using Accord;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDietPlan.Models
{
    public class createDataSet
    {
        //public static string path = Path.Combine(Environment.CurrentDirectory, "Data", "Simplified Nutrition Facts.csv");
        //public static string PathFile = Path.Combine(Environment.CurrentDirectory, "Data", "DataSetFood.txt");
        //public static string PathTest = Path.Combine(Environment.CurrentDirectory, "Data", "FoodTest.txt");

        public static string path = Path.Combine(Environment.CurrentDirectory, "Data", "ID3DataSet.csv");
        public static string PathFile = Path.Combine(Environment.CurrentDirectory, "Data", "ID3DataV2.csv");
       
        public createDataSet()
        {
            var csv = new CsvReader(File.OpenText(path));
            var myCustomObjects = csv.GetRecords<MealData>();

            DataTable dt = new DataTable("ID3");
            DataRow row;

            dt.Columns.Add("fgroup", "Category", "Carb", "Protein", "Fat", "Fiber", "Calorie", "Outcome");
            foreach(var record in myCustomObjects)
            {
                row = dt.NewRow();
             
                row["Category"] = record.Category;
                row["Carb"] = record.Carb;
                row["Protein"] = record.Protein;
                row["Fat"] = record.Fat;
                row["Fiber"] = record.Fiber;
                row["Calorie"] = record.Calorie;

                if(record.Category.Equals("normal"))
                {
                    if(record.Carb.Equals("low") && record.Protein.Equals("low")&&record.Fat.Equals("low")&&record.Fiber.Equals("low")&&record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else
                    {
                        row["Outcome"] = "positive";
                    }
                }
                if(record.Category.Equals("overweight"))
                {
                    if (record.Carb.Equals("low") && record.Fat.Equals("low")&&record.Protein.Equals("low")&& record.Fiber.Equals("low")&& record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if(record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                }
                if (record.Category.Equals("obese"))
                {
                    if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                }
                if (record.Category.Equals("morbid"))
                {
                    if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("low") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("low") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("low") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("low") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("low"))
                    {
                        row["Outcome"] = "positive";
                    }
                    else if (record.Carb.Equals("high") && record.Fat.Equals("high") && record.Protein.Equals("high") && record.Fiber.Equals("high") && record.Calorie.Equals("high"))
                    {
                        row["Outcome"] = "negative";
                    }
                }
                dt.Rows.Add(row);
            }

            //DataTable dt = new DataTable("FoodUsda");
            //DataRow row;

            //dt.Columns.Add("FGroup", "Protein", "Fat", "Carb", "Calorie", "Fiber","Lev_Protein", "Lev_Fat", "Lev_Carb", "Lev_calorie", "Lev_Fiber", "Decision");
            //foreach (var record in myCustomObjects)
            //{
            //    row = dt.NewRow();


            //    row["FGroup"] = record.FGroup;
            //    row["Protein"] = record.protein;
            //    row["Carb"] = record.carb;
            //    row["Fat"] = record.fat;
            //    row["Fiber"] = record.fiber;
            //    row["Calorie"] = record.calorie;

            //if(record.protein < 10f)
            //{
            //    row["Lev_Protein"] = "low";

            //}
            //else
            //{
            //    row["Lev_Protein"] = "high";
            //}
            //if(record.carb < 10f)
            //{
            //    row["Lev_Carb"] = "low";
            //}
            //else
            //{
            //    row["Lev_Carb"] = "high";
            //}
            //if (record.fat < 5f)
            //{
            //    row["Lev_Fat"] = "low";
            //}
            //else
            //{
            //    row["Lev_Fat"] = "high";
            //}
            //if (record.calorie < 100f)
            //{
            //    row["Lev_Calorie"] = "low";
            //}
            //else
            //{
            //    row["Lev_Calorie"] = "high";
            //}
            //if (record.fiber < 1f)
            //{
            //    row["Lev_Fiber"] = "low";
            //}
            //else
            //{

            //    row["Lev_Fiber"] = "high";

            //}

            //    if (record.protein < 15f)
            //    {
            //        row["Lev_Protein"] = "low";

            //    }
            //    else
            //    {
            //        row["Lev_Protein"] = "high";
            //    }
            //    if (record.carb < 15f)
            //    {
            //        row["Lev_Carb"] = "low";
            //    }
            //    else
            //    {
            //        row["Lev_Carb"] = "high";
            //    }
            //    if (record.fat < 5f)
            //    {
            //        row["Lev_Fat"] = "low";
            //    }
            //    else
            //    {
            //        row["Lev_Fat"] = "high";
            //    }
            //    if (record.calorie < 100f)
            //    {
            //        row["Lev_Calorie"] = "low";
            //    }
            //    else
            //    {
            //        row["Lev_Calorie"] = "high";
            //    }
            //    if (record.fiber < 1f)
            //    {
            //        row["Lev_Fiber"] = "low";
            //    }
            //    else
            //    {

            //        row["Lev_Fiber"] = "high";

            //    }
            //    row["Decision"] = row["Lev_Protein"].ToString() + "," + row["Lev_Carb"].ToString() + "," + row["Lev_Fat"].ToString() + "," + row["Lev_Calorie"].ToString() + "," + row["Lev_Fiber"].ToString();
            //    dt.Rows.Add(row);
            //}
            //foreach (DataRow rows in dt.Rows)
            //{
            //    System.Diagnostics.Debug.WriteLine("Results: " + rows["FGroup"] + ": " + rows["Decision"].ToString());
            //}

            //TextWriter tw = new StreamWriter(PathFile);
            //foreach (DataRow rows in dt.Rows)
            //{
            //    tw.WriteLine(rows["FGroup"].ToString() + "*" + rows["Protein"].ToString() + "*" + rows["Carb"].ToString() + "*" + rows["Fat"].ToString() + "*" + rows["Calorie"].ToString() + "*" + rows["Fiber"].ToString() + "*" + rows["Decision"].ToString());
            //}

            //TextWriter tw = new StreamWriter(PathTest);
            //int count = 0;
            //foreach (DataRow rows in dt.Rows)
            //{
            //    count++;
            //    tw.WriteLine(rows["FGroup"].ToString() + "*" + rows["Protein"].ToString() + "*" + rows["Carb"].ToString() + "*" + rows["Fat"].ToString() + "*" + rows["Calorie"].ToString() + "*" + rows["Fiber"].ToString() + "*" + rows["Decision"].ToString());
            //    if (count == 150)
            //    {
            //        break;
            //    }
            //}
            using (StreamWriter sw = File.CreateText(PathFile))
            {
                foreach (DataRow rows in dt.Rows)
                {
                    
                   sw.WriteLine(rows["Category"].ToString()+","+rows["Carb"].ToString()+","+rows["Protein"].ToString()+","+rows["Fat"].ToString()+","+rows["Fiber"].ToString()+","+rows["Calorie"].ToString()+","+rows["Outcome"].ToString());
                    
                }
            }
            
        }

    }
}
