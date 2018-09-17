using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Analytics.Model.SalesForecasting
{
    public class ProductSales : IProductSales
    {
        /// <summary>
        /// This method demonstrates how to run prediction on one example at a time.
        /// </summary>
        public async Task<ProductUnitPrediction> Predict(string modelPath, string productId, int year, int month, float units, float avg, int count, float max, float min, float prev)
        {
            // Load model
            var predictionEngine = await CreatePredictionEngineAsync(modelPath);

            // Build country sample
            var inputExample = new ProductData(productId, year, month, units, avg, count, max, min, prev);

            // Returns prediction
            return predictionEngine.Predict(inputExample);
        }

        /// <summary>
        /// This function creates a prediction engine from the model located in the <paramref name="modelPath"/>.
        /// </summary>
        private async Task<PredictionModel<ProductData, ProductUnitPrediction>> CreatePredictionEngineAsync(string modelPath)
        {
            PredictionModel<ProductData, ProductUnitPrediction> model = await PredictionModel.ReadAsync<ProductData, ProductUnitPrediction>(modelPath);
            return model;
        }
    }
}
