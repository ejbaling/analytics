﻿using Analytics.Model.SalesForecasting;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SalesForecasting.Training
{
    public class ProductModelHelper
    {
        /// <summary>
        /// Train and save model for predicting next month country unit sales
        /// </summary>
        /// <param name="dataPath">Input training file path</param>
        /// <param name="outputModelPath">Trained model path</param>
        public static async Task SaveModel(string dataPath, string outputModelPath = "product_month_fastTreeTweedie.zip")
        {
            if (File.Exists(outputModelPath))
            {
                File.Delete(outputModelPath);
            }

            var model = CreateProductModelUsingPipeline(dataPath);

            await model.WriteAsync(outputModelPath);
        }


        /// <summary>
        /// Build model for predicting next month country unit sales using Learning Pipelines API
        /// </summary>
        /// <param name="dataPath">Input training file path</param>
        /// <returns></returns>
        private static PredictionModel<ProductData, ProductUnitPrediction> CreateProductModelUsingPipeline(string dataPath)
        {
            if (!File.Exists(dataPath))
                Console.WriteLine("File does not exist.");

            Console.WriteLine("*************************************************");
            Console.WriteLine("Training product forecasting model using Pipeline");

            var learningPipeline = new LearningPipeline();

            // First stage in the pipeline will be reading the source csv file
            learningPipeline.Add(new TextLoader(dataPath).CreateFrom<ProductData>(useHeader: true, separator: ','));

            // The model needs the columns to be arranged into a single column of numeric type
            // First, we group all numeric columns into a single array named NumericalFeatures
            learningPipeline.Add(new ColumnConcatenator(
                "NumericalFeatures", 
                nameof(ProductData.Year),
                nameof(ProductData.Month),
                nameof(ProductData.Max),
                nameof(ProductData.Min),
                nameof(ProductData.Count),
                nameof(ProductData.Units),
                nameof(ProductData.Avg),
                nameof(ProductData.Prev)
            ));

            // Second group is for categorical features (just one in this case), we name this column CategoryFeatures
            learningPipeline.Add(new ColumnConcatenator(outputColumn: "CategoryFeatures", nameof(ProductData.ProductId)));

            // Then we need to transform the category column using one-hot encoding. This will return a numeric array
            learningPipeline.Add(new CategoricalOneHotVectorizer("CategoryFeatures"));

            // Once all columns are numeric types, all columns will be combined
            // into a single column, named Features 
            learningPipeline.Add(new ColumnConcatenator(outputColumn: "Features", "NumericalFeatures", "CategoryFeatures"));

            // Add the Learner to the pipeline. The Learner is the machine learning algorithm used to train a model
            // In this case, TweedieFastTree.TrainRegression was one of the best performing algorithms, but you can 
            // choose any other regression algorithm (StochasticDualCoordinateAscentRegressor,PoissonRegressor,...)
            learningPipeline.Add(new FastTreeTweedieRegressor { NumThreads = 1, FeatureColumn = "Features" });

            // Finally, we train the pipeline using the training dataset set at the first stage
            var model = learningPipeline.Train<ProductData, ProductUnitPrediction>();

            return model;
        }

        /// <summary>
        /// Predict samples using saved model
        /// </summary>
        /// <param name="outputModelPath">Model file path</param>
        /// <returns></returns>
        public static async Task TestPrediction(string outputModelPath = "product_month_fastTreeTweedie.zip")
        {
            Console.WriteLine("*********************************");
            Console.WriteLine("Testing product forecasting model");

            // Read the model that has been previously saved by the method SaveModel
            var model = await PredictionModel.ReadAsync<ProductData, ProductUnitPrediction>(outputModelPath);

            // Build sample data
            ProductData dataSample = new ProductData()
            {
                ProductId = "428",
                Year = 2018,
                Month = 8,
                Units = 404,
                Avg = 5.179487f,
                Count = 78,
                Max = 60,
                Min = 1,
                Prev = 335
            };

            // Predict sample data
            ProductUnitPrediction prediction = model.Predict(dataSample);
            Console.WriteLine($"Product: {dataSample.ProductId}, month: {dataSample.Month + 1}, year: {dataSample.Year} - Real value: {dataSample.Units}, Forecasting: {prediction.Score}");

            dataSample = new ProductData()
            {
                ProductId = "428",
                Year = 2018,
                Month = 9,
                Units = 302,
                Avg = 5.206896f,
                Count = 58,
                Max = 40,
                Min = 1,
                Prev = 404
            };

            prediction = model.Predict(dataSample);
            Console.WriteLine($"Product: {dataSample.ProductId}, month: {dataSample.Month + 1}, year: {dataSample.Year} - Forecasting: {prediction.Score}");
        }
    }
}
