using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accord.MachineLearning.Rules;
using System.IO;
using System.Data;
using CsvHelper;
using Accord;
using Accord.Statistics.Filters;
using Accord.Math;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.MachineLearning.DecisionTrees;
using Accord.Math.Optimization.Losses;
using Microsoft.AspNetCore.Http;
using Accord.MachineLearning;
using Accord.Statistics.Analysis;

namespace CoreDietPlan.Models
{
  
   
    public class ID3Tree
    {
        //public static string path = Path.Combine(Environment.CurrentDirectory, "Data", "ID3DataSet.csv");
        public static string path = Path.Combine(Environment.CurrentDirectory, "Data", "ID3DataV2.csv");
        public static string basePath = Path.Combine(Environment.CurrentDirectory, "Data");
        string BMI = "";
        string User = "";
        public List<Meal> ID3(dynamic food,List<User>users)
        {

            //dtable();
            // Dictionary<string,List<Category>> FoodCategory = new Dictionary<string,List<Category>>();
            List<Meal> category = new List<Meal> { };
            foreach (User u in users)
            {
                BMI = u.bmi;
                User = u.user;
            }


            //Iterate through food and add each food with range of macronutrients to category list
            foreach (var nw in food)
            {
                var value = (nw.Category.Predicted).Split(",");
                category.Add( new Meal {food=nw.Food.Food,protein=value[0],carb=value[1],fat=value[2],calorie=value[3],fiber=value[4],nprotein=nw.Food.Total_Protein,ncarb=nw.Food.Total_Carb,nfat=nw.Food.Total_Fat,ncalorie=nw.Food.Num_Calorie,nfiber=nw.Food.Fiber,serving = nw.Food.Serving, img = nw.Food.img, fgroup = nw.Food.fgroup,PrefID=nw.Food.ID, servingQty = nw.Food.ServingQty.ToString(), decision ="" });
                
            }
           
            var csv = new CsvReader(File.OpenText(path));
            var myCustomObjects = csv.GetRecords<MealData>();

            DataTable dt = new DataTable("FoodDBSample");
            DataRow row;

            dt.Columns.Add("Category", "Carb", "Protein", "Fat", "Calorie", "Fiber", "Decision");
            foreach (var record in myCustomObjects)
            {
                row = dt.NewRow();

             
                row["Category"] = record.Category;
                row["Carb"] = record.Carb;
                row["Protein"] = record.Protein;
                row["Fat"] = record.Fat;
                row["Calorie"] = record.Calorie;
                row["Fiber"] = record.Fiber;
                row["Decision"] = record.Outcome;

                dt.Rows.Add(row);
            }
            var codebook = new Codification(dt);

            DataTable symbols = codebook.Apply(dt);

            int[][] inputs = symbols.ToJagged<int>("Category", "Carb", "Protein", "Fat", "Calorie", "Fiber");
            int[] outputs = symbols.ToArray<int>("Decision");

            //specify which columns to use for making decisions
            var id3learning = new ID3Learning()
                     {
                         new DecisionVariable("Category",     4),
                         new DecisionVariable("Carb", 2),
                         new DecisionVariable("Protein",    2),
                         new DecisionVariable("Fat",        2),
                         new DecisionVariable("Calorie",        2),
                         new DecisionVariable("Fiber",        2)
                     };


            // Learn the training instances!
            DecisionTree tree = id3learning.Learn(inputs, outputs);

            // Compute the training error when predicting training instances
            double error = new ZeroOneLoss(outputs).Loss(tree.Decide(inputs));

            

            List<dynamic> predict = new List<dynamic> { };
           //iterate through data structure category and make prediction for each food
            foreach (var item in category)
            {
                predict.Add(codebook.Transform(new[,]
                {
                         { "Category",     $"{BMI}"  },
                         { "Carb", $"{item.carb}"    },
                         { "Protein", $"{item.protein}"   },
                         { "Fat",   $"{item.fat}" },
                         { "Calorie",   $"{item.calorie}" },
                         { "Fiber",     $"{item.fiber}" }
                    }));
            }

            //Accord.IO.Serializer.Save(tree, Path.Combine(basePath, "ID3TreeModel.bin"));
            //int predicted = tree.Decide(query);
            List<string> q = new List<string>();
            foreach (var i in predict)
            {
                q.Add(codebook.Revert("Decision", tree.Decide(i)));
            }
            // We can translate it back to strings using
            //string answer = codebook.Revert("Decision", predicted);
            //foreach (var item in q)
            //{
            //    System.Diagnostics.Debug.WriteLine(item);
            //}
            var foods = category.Zip(q, (n, w) => new { Food = n, Decision = w });
            List<Meal> positive = new List<Meal> { };
            List<Meal> negative = new List<Meal> { };
            foreach (var nw in foods)
            {
                nw.Food.decision = nw.Decision;
            }
            foreach (var item in foods)
            {
                //System.Diagnostics.Debug.WriteLine(item.Food.food + "\n" + item.Food.nprotein + "\n" + item.Food.ncarb + "\n" + item.Food.nfat + "\n" + item.Food.ncalorie + "\n" + item.Food.serving + "\n" + item.Food.img + "\n" + item.Food.fgroup+"\n"+item.Decision);
                //System.Diagnostics.Debug.WriteLine(item.Food.food+"\n"+item.Decision);
                if (item.Decision.Equals("positive"))
                {
                    positive.Add(new Meal { CurrentUser=User,food = item.Food.food,nprotein=item.Food.nprotein,ncarb=item.Food.ncarb,nfat=item.Food.nfat,ncalorie=item.Food.ncalorie,nfiber=item.Food.nfiber,img=item.Food.img,serving=item.Food.serving,fgroup=item.Food.fgroup,PrefID=item.Food.PrefID, servingQty = item.Food.servingQty, decision = item.Decision });
                }
                if (item.Decision.Equals("negative"))
                {
                    negative.Add(new Meal { CurrentUser = User, food = item.Food.food, nprotein = item.Food.nprotein, ncarb = item.Food.ncarb, nfat = item.Food.nfat, ncalorie = item.Food.ncalorie, nfiber = item.Food.nfiber, img = item.Food.img, serving = item.Food.serving, fgroup = item.Food.fgroup,PrefID=item.Food.PrefID, servingQty = item.Food.servingQty, decision = item.Decision });
                }
            }
            //foreach (var item in positive)
            //{

            //    //System.Diagnostics.Debug.WriteLine(item.food + "\n" + item.nprotein + "\n" + item.ncarb + "\n" + item.nfat + "\n" + item.ncalorie + "\n" + item.serving + "\n" + item.img + "\n" + item.fgroup + "\n" + item.decision);
            //    System.Diagnostics.Debug.WriteLine(item.food + "\n" + "Protein: "+item.nprotein + "\n" + "Carb: " + item.ncarb + "\n" + "Fat: " + item.nfat + "\n" + "Calorie: " + item.ncalorie+"\n"+ "Fiber: "+item.nfiber+"\n"+ "Outcome: " + item.decision);

            //}
            //foreach (var item in negative)
            //{

            //    //System.Diagnostics.Debug.WriteLine(item.food + "\n" + item.nprotein + "\n" + item.ncarb + "\n" + item.nfat + "\n" + item.ncalorie + "\n" + item.serving + "\n" + item.img + "\n" + item.fgroup + "\n" + item.decision);
            //    System.Diagnostics.Debug.WriteLine(item.food + "\n" + "Protein: " + item.nprotein + "\n" + "Carb: " + item.ncarb + "\n" + "Fat: " + item.nfat + "\n" + "Calorie: " + item.ncalorie + "\n" + "Fiber: " + item.nfiber + "\n" + "Outcome: " + item.decision);

            //}


            // Validation purposes only 
            //var cm = GeneralConfusionMatrix.Estimate(tree, inputs, outputs);
            //double err = cm.Error;  
            //double acc = cm.Accuracy;  
            //double kappa = cm.Kappa;
            //validation();
            //System.Diagnostics.Debug.WriteLine("error: " + err);
            //System.Diagnostics.Debug.WriteLine("accuracy: " + acc);
            DisplayDiet diet = new DisplayDiet();
            diet.GetFoods(positive);
            diet.GetUser(users);
            //diet.CreateDiet(positive);
            return positive;
        }
        public void validation()
        {
         
            var data = path;
           
            var csv = new CsvReader(File.OpenText(path));
            var myCustomObjects = csv.GetRecords<MealData>();

            DataTable dt = new DataTable("FoodDBSample");
            DataRow row;

            dt.Columns.Add("Category", "Carb", "Protein", "Fat", "Calorie", "Fiber", "Decision");
            foreach (var record in myCustomObjects)
            {
                row = dt.NewRow();


                row["Category"] = record.Category;
                row["Carb"] = record.Carb;
                row["Protein"] = record.Protein;
                row["Fat"] = record.Fat;
                row["Calorie"] = record.Calorie;
                row["Fiber"] = record.Fiber;
                row["Decision"] = record.Outcome;

                dt.Rows.Add(row);
            }
            var codebook = new Codification(dt);

            DataTable symbols = codebook.Apply(dt);

            int[][] inputs = symbols.ToJagged<int>("Category", "Carb", "Protein", "Fat", "Calorie", "Fiber");
            int[] outputs = symbols.ToArray<int>("Decision");

            //specify which columns to use for making decisions
            var id3learning = new ID3Learning()
                     {
                         new DecisionVariable("Category",     4),
                         new DecisionVariable("Carb", 2),
                         new DecisionVariable("Protein",    2),
                         new DecisionVariable("Fat",        2),
                         new DecisionVariable("Calorie",        2),
                         new DecisionVariable("Fiber",        2)
                     };


            
            DecisionTree tree = id3learning.Learn(inputs, outputs);

            // Compute the training error
            double error = new ZeroOneLoss(outputs).Loss(tree.Decide(inputs));

            // measure the cross-validation performance of
            // a decision tree with a maximum tree height of 5. With variables
            // able to join the decision path at most 2 times during evaluation:
            var cv = CrossValidation.Create(

                k: 5, // 5-fold cross-validation

                learner: (p) => new ID3Learning() //create the learning algorithm
               {
                     new DecisionVariable("Category",     4),
                         new DecisionVariable("Carb", 2),
                         new DecisionVariable("Protein",    2),
                         new DecisionVariable("Fat",        2),
                         new DecisionVariable("Calorie",        2),
                         new DecisionVariable("Fiber",        2)
                },

                
                loss: (actual, expected, p) => new ZeroOneLoss(expected).Loss(actual),

                // function can be used to perform any special
                // operations before the actual learning is done, but
                // here we will just leave it as simple as it can be:
                fit: (teacher, x, y, w) => teacher.Learn(x, y, w),

                // pass the input and output data
                // that will be used in cross-validation. 
                x: inputs, y: outputs
            );

            // After the cross-validation object has been created,
            // we can call its .Learn method with the input and 
            // output data that will be partitioned into the folds:
            var result = cv.Learn(inputs, outputs);

            //Gather info
            int numberOfSamples = result.NumberOfSamples; 
            int numberOfInputs = result.NumberOfInputs;   
            int numberOfOutputs = result.NumberOfOutputs; 

            double trainingError = result.Training.Mean; 
            double validationError = result.Validation.Mean; 

            System.Diagnostics.Debug.WriteLine("ID3 Mean: " + validationError);
            System.Diagnostics.Debug.WriteLine("ID3 Error: " + trainingError);
        }

    }
}
