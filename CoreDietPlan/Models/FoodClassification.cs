using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML;
using Microsoft.ML.Transforms;
using Microsoft.ML.Models;
using System.Globalization;

namespace CoreDietPlan.Models
{
    public class FoodClassification
    {
        //static readonly string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "FoodStochasticData.txt");
        static readonly string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "DataSetFood.txt");
        //static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "FoodStochasticModel.zip");
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "FoodDSetModel.zip");
        //static readonly string _testdataPath = Path.Combine(Environment.CurrentDirectory, "Data", "FoodData.txt");
        static readonly string _testdataPath = Path.Combine(Environment.CurrentDirectory, "Data", "Foodtestdata.txt");
        public FoodClassification(List<User>patient)
        {
            Main(patient).GetAwaiter().GetResult();
        }

        public static async Task Main(List<User> patient)
        {
            //call Train method
            PredictionModel<FoodCustomClass, ClassPrediction> model = Train();
            await model.WriteAsync(_modelPath);

            //Call FoodClassificationTest class for prediction
            var prediction = model.Predict(FoodClassificationTest.FoodClassification());
           
            //Store each food in a list
            List<FoodCustomClass>foods=FoodClassificationTest.FoodClassification();

            var testData = new TextLoader(_testdataPath).CreateFrom<FoodCustomClass>(separator: '*');
            var evaluator = new ClassificationEvaluator();
            ClassificationMetrics metrics = evaluator.Evaluate(model, testData);

            // Displaying the metrics for model validation
            System.Diagnostics.Debug.WriteLine("PredictionModel quality metrics evaluation");
            System.Diagnostics.Debug.WriteLine("------------------------------------------");
            System.Diagnostics.Debug.WriteLine($"*       MicroAccuracy:    {metrics.AccuracyMicro:0.###}");
            System.Diagnostics.Debug.WriteLine($"*       MacroAccuracy:    {metrics.AccuracyMacro:0.###}");
            System.Diagnostics.Debug.WriteLine($"*       LogLoss:          {metrics.LogLoss:#.###}");
            System.Diagnostics.Debug.WriteLine($"*       LogLossReduction: {metrics.LogLossReduction:#.###}");


            //Combine List foods with their respective results after classification
            /* 
            foodsAndClassification contain each food from FoodDB database classified in terms 
            of the quantity of each macronutrient present
            */
            var foodsAndClassification = foods.Zip(prediction, (n, w) => new { Food = n, Category = w });
           
            //call ID3Tree.cs and send foodsAndClassification as parameter to ID3Tree constructor
            var function = new ID3Tree().ID3(foodsAndClassification, patient);

            //foreach (var nw in foodsAndClassification)
            //{
            //    System.Diagnostics.Debug.WriteLine($"{nw.Food.Food}: {nw.Category.Predicted}" +"\n" + $"{float.Parse(nw.Category.Score.First().ToString(), CultureInfo.InvariantCulture.NumberFormat)}");
            //}


        }
        public static PredictionModel<FoodCustomClass, ClassPrediction> Train()
        {

            var pipeline = new LearningPipeline();
            pipeline.Add(new TextLoader(_dataPath).CreateFrom<FoodCustomClass>(separator: '*'));

            pipeline.Add(new Dictionarizer("Label"));
           

            pipeline.Add(new ColumnConcatenator("Features", "Total_Protein", "Total_Carb", "Total_Fat", "Num_Calorie","Fiber"));

            pipeline.Add(new StochasticDualCoordinateAscentClassifier());

            pipeline.Add(new PredictedLabelColumnOriginalValueConverter() { PredictedLabelColumn = "PredictedLabel" });
         

            var model = pipeline.Train<FoodCustomClass, ClassPrediction>();

            //var testData = new TextLoader(_testdataPath).CreateFrom<FoodCustomClass>(separator: '*');
            //var evaluator = new ClassificationEvaluator();
            //ClassificationMetrics metrics = evaluator.Evaluate(model, testData);

            //// Displaying the metrics for model validation
            //System.Diagnostics.Debug.WriteLine("PredictionModel quality metrics evaluation");
            //System.Diagnostics.Debug.WriteLine("------------------------------------------");
            //System.Diagnostics.Debug.WriteLine($"*       MicroAccuracy:    {metrics.AccuracyMicro:0.###}");
            //System.Diagnostics.Debug.WriteLine($"*       MacroAccuracy:    {metrics.AccuracyMacro:0.###}");
            //System.Diagnostics.Debug.WriteLine($"*       LogLoss:          {metrics.LogLoss:#.###}");
            //System.Diagnostics.Debug.WriteLine($"*       LogLossReduction: {metrics.LogLossReduction:#.###}");
            //var crossValidator = new CrossValidator()
            //{
            //    Kind = MacroUtilsTrainerKinds.SignatureMultiClassClassifierTrainer,
            //    NumFolds = 3
            //};

            //var crossValidatorOutput = crossValidator.CrossValidate<FoodCustomClass, ClassPrediction>(pipeline);

            //crossValidatorOutput.RegressionMetrics.ForEach(m => System.Diagnostics.Debug.WriteLine(m.Rms));

            //var totalR2 = crossValidatorOutput.RegressionMetrics.Sum(metric => metric.RSquared);
            //var totalRMS = crossValidatorOutput.RegressionMetrics.Sum(metric => metric.Rms);

            //System.Diagnostics.Debug.WriteLine(Environment.NewLine);
            //System.Diagnostics.Debug.WriteLine($"Average R^2: {totalR2 / crossValidatorOutput.RegressionMetrics.Count}");
            //System.Diagnostics.Debug.WriteLine($"Average RMS: {totalRMS / crossValidatorOutput.RegressionMetrics.Count}");


            return model;

        }
       
    }
}
