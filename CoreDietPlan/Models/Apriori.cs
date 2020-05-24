using System.Collections.Generic;
using Accord.MachineLearning.Rules;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System;

namespace CoreDietPlan.Models
{

    public class Apriori
    {

        public string[][] AprioriAI(string[]BreakFastDietplan, string[]LunchDietplan,string[]DinnerDietplan)
        {
           string path= Path.Combine(Environment.CurrentDirectory, "Data", "TeachPreferences.txt");
           string _Path = Path.Combine(Environment.CurrentDirectory, "Data", "PreferencesDB.txt");

            // Let the database of dietplan consist of following itemsets:

            //SortedSet<string>[] dataset =
            //{
            //// Each row represents a set of foods that have been consumed 
            //// together. Each number is a SKU identifier for a food.
            //new SortedSet<string> { "grilled chicken breast", "lettuce", "carrot", "3 oz of greek yogurt" }, //consumed 4 food as a meal
            //new SortedSet<string> { "grilled chicken breast", "lettuce", "3 oz of greek yogurt" },    // consumed 3 food as a meal
            //new SortedSet<string> { "grilled chicken breast", "lettuce" },       // ....
            //new SortedSet<string> { "lettuce", "carrot", "3 oz of greek yogurt" },
            //new SortedSet<string> { "lettuce", "carrot" },
            //new SortedSet<string> { "carrot", "3 oz of greek yogurt" },
            //new SortedSet<string> { "lettuce", "3 oz of greek yogurt" },
            //};


            //TextWriter tw = new StreamWriter(_Path,true);

            //tw.Write(string.Join(",", BreakFastDietplan));
            //tw.WriteLine("");
            //tw.Write(string.Join(",", LunchDietplan));
            //tw.WriteLine("");
            //tw.Write(string.Join(",", DinnerDietplan));

            //tw.Close();

            //if (File.Exists(_Path))
            //{
            //    string lines; 
          
            //    System.IO.StreamReader f = new System.IO.StreamReader(_Path);
            //    while ((lines = f.ReadLine()) != null)
            //    {
            //        if (lines==string.Join(",",BreakFastDietplan))
            //        {

            //        }

            //    }
            //}

            SortedSet<string>[] resultingArray = new SortedSet<string>[3];

            string line; int counter = 0;
            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(_Path);
            while ((line = file.ReadLine()) != null)
            {
                line = line.Replace(" ", string.Empty); //remove unnecessary spaces
                SortedSet<string> lineSet = new SortedSet<string>(line.Split(','));
                resultingArray.SetValue(lineSet, counter);

                counter++;

            }


            // We will use Apriori to determine the frequent item sets of this database.
            // To do this, we will say that an item set is frequent if it appears in at 
            // least 3 transactions of the database: the value 3 is the support threshold.

            // Create a new a-priori learning algorithm with support 3
            var apriori = new Apriori<string>(threshold: 2, confidence: 0);

            // Use the algorithm to learn a set matcher
            AssociationRuleMatcher<string> classifier = apriori.Learn(resultingArray);

            // Use the classifier to find orders that are similar to 
            // orders where clients have bought items 1 and 2 together:
            //string[][] matches = classifier.Decide(new[] { "2 oz. grilled chicken breast", "lettuce" });//this is a jagged array
            string[] foodtest = { "4", "9" };
            string[][] matches = classifier.Decide(foodtest);


            // We can also obtain the association rules from frequent itemsets:
            AssociationRule<string>[] rules = classifier.Rules;
            var food = new List<Meal> { };
            //foreach (string[] i in matches)
            //{
            //    foreach (string item in i)
            //    {
            //        System.Diagnostics.Debug.WriteLine("Results: " + item);
            //    }

            //}
            return matches;
        }

    }
}
