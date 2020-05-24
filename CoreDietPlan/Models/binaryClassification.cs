using System.Collections.Generic;
using CsvHelper;
using System.IO;
using Accord.MachineLearning;
using System;
using Accord.MachineLearning.Rules;
using System.Data;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML;
using Microsoft.ML.Transforms;
using System.Threading.Tasks;


namespace CoreDietPlan.Models
{
    public class binaryClassification
    {
        static readonly string _trainDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "usda.txt");
        static readonly string _testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "usdaTest.txt");
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");
      

        public binaryClassification()
        {
            BC().GetAwaiter().GetResult();
        }

        public static async Task BC()
        {
            PredictionModel<FoodBinaryClass, FPrediction> model = Train();
            await model.WriteAsync(_modelPath);
            var prediction = model.Predict(binaryTest.meal);
            System.Diagnostics.Debug.WriteLine($"Decision: {prediction.Prediction}");
            System.Diagnostics.Debug.WriteLine($"Probability: {prediction.Probability}");
            System.Diagnostics.Debug.WriteLine($"Score: {prediction.Score}");
        }
        public static PredictionModel<FoodBinaryClass, FPrediction> Train()
        {

            var pipeline = new LearningPipeline();
            pipeline.Add(new TextLoader(_trainDataPath).CreateFrom<FoodBinaryClass>(separator: ','));

            pipeline.Add(new Dictionarizer("Label"));

            pipeline.Add(new TextFeaturizer("Features", "meal","Protein","Carb","Fat","Calorie","Preference"));
      
            pipeline.Add(new FastTreeBinaryClassifier() { NumLeaves = 5, NumTrees = 5, MinDocumentsInLeafs = 2 });
            pipeline.Add(new PredictedLabelColumnOriginalValueConverter() { PredictedLabelColumn = "PredictedLabel" });

            var model = pipeline.Train<FoodBinaryClass, FPrediction>();

            return model;

        }

    }
}
