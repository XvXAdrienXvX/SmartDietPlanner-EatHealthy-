using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Runtime.Api;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CoreDietPlan
{
    //STEP 1: Define the data structure. That is classes to hold the data
    public class WeightData
    {
        [Column("0")]
        public float BMI;

        [Column("1")]
        public float WC;

      
        [Column("2")]
        [ColumnName("Label")]
        public string Label;
    }

    public class WeightPrediction
    {
        [ColumnName("PredictedLabel")]
        public string PredictedLabels;
    }
    public class WeightClassification
    {
        static PredictionModel<WeightData, WeightPrediction> TrainedModel;
        static readonly string dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "Weight-data.txt");
        static readonly string modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "WeightModel.txt");
        public static string category = "";

        public static string returnClassification(  float userBMI, float userWC)
        {
            Main(userBMI, userWC).GetAwaiter().GetResult();

            return category;
        }

        public static async Task Main( float userBMI, float userWC)
        {
           // Use the trained model to classify the user based on their details

            var prediction = TrainedModel.Predict(new WeightData()
            {
                BMI = userBMI,
                WC = userWC
            });
            category = prediction.PredictedLabels;
        }

              /*The below function is used to train the application based on the dataset provided
                This function is called on startup of the application and the result is stored in a static model
             */
        public static void Train()
        {

            var pipeline = new LearningPipeline();
            pipeline.Add(new TextLoader(dataPath).CreateFrom<WeightData>(separator: ','));
            //Transform data
            pipeline.Add(new Dictionarizer("Label"));
            pipeline.Add(new ColumnConcatenator("Features", "BMI", "WC"));

            //add learning/training algo
            pipeline.Add(new StochasticDualCoordinateAscentClassifier());
            pipeline.Add(new PredictedLabelColumnOriginalValueConverter() { PredictedLabelColumn = "PredictedLabel" });


            var model = pipeline.Train<WeightData, WeightPrediction>();

            TrainedModel = model;

        }
    }
}
