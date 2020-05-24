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
using Accord.Math;

namespace CoreDietPlan.Models
{
    public class KMeansAlgo
    {
        static readonly string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "FoodSample.txt");
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "MealModel.zip");

        public KMeansAlgo()
        {
            KM().GetAwaiter().GetResult();
        }

        public static async Task KM()
        {
            PredictionModel<MealClass, ClusterPrediction> model = Train();
            await model.WriteAsync(_modelPath);
            var prediction = model.Predict(KMeansTest.meal);
            System.Diagnostics.Debug.WriteLine($"Category: {prediction.PredictedCategory}");
          
        }
        public static PredictionModel<MealClass, ClusterPrediction> Train()
        {
           
            var pipeline = new LearningPipeline();
            pipeline.Add(new TextLoader(_dataPath).CreateFrom<MealClass>(separator: ','));

            //pipeline.Add(new Dictionarizer("Label"));
            pipeline.Add(new TextFeaturizer("Features", "category"));

            pipeline.Add(new ColumnConcatenator("Features", "carb", "protein","fat", "calorie"));

            pipeline.Add(new KMeansPlusPlusClusterer() { K = 5 });

            //pipeline.Add(new PredictedLabelColumnOriginalValueConverter() { PredictedLabelColumn = "PredictedLabel" });

            var model = pipeline.Train<MealClass, ClusterPrediction>();

            return model;

        }


    }
}
