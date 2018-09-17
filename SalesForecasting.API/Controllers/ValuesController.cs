using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using SalesForecasting.Training.Models;
using System.Collections.Generic;

namespace SalesForecasting.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get(int productId, int year, int month, int max, int min, int count, int units, int avg, int prev)
        {
            // Create a pipeline and load your data
            var pipeline = new LearningPipeline();

            // If working in Visual Studio, make sure the 'Copy to Output Directory' 
            // property of weather-data.txt is set to 'Copy always'
            string dataPath = "product-data.txt";
            pipeline.Add(new TextLoader(dataPath).CreateFrom<ProductData>(false, separator: ','));

            // The model needs the columns to be arranged into a single column of numeric type
            // First, we group all numeric columns into a single array named NumericalFeatures
            pipeline.Add(new ColumnConcatenator("NumericalFeatures", nameof(ProductData.Year),
                nameof(ProductData.Month),
                nameof(ProductData.Max),
                nameof(ProductData.Min),
                nameof(ProductData.Count),
                nameof(ProductData.Units),
                nameof(ProductData.Avg),
                nameof(ProductData.Prev)
            ));

            // Second group is for categorical features (just one in this case), we name this column CategoryFeatures
            pipeline.Add(new ColumnConcatenator("CategoryFeatures", nameof(ProductData.ProductId)));

            // Then we need to transform the category column using one-hot encoding. This will return a numeric array
            pipeline.Add(new CategoricalOneHotVectorizer("CategoryFeatures"));

            // Once all columns are numeric types, all columns will be combined
            // into a single column, named Features 
            pipeline.Add(new ColumnConcatenator("Features", "NumericalFeatures", "CategoryFeatures"));

            // Add the Learner to the pipeline. The Learner is the machine learning algorithm used to train a model
            // In this case, TweedieFastTree.TrainRegression was one of the best performing algorithms, but you can 
            // choose any other regression algorithm (StochasticDualCoordinateAscentRegressor,PoissonRegressor,...)
            pipeline.Add(new FastTreeTweedieRegressor { NumThreads = 1, FeatureColumn = "Features" });

            // Finally, we train the pipeline using the training dataset set at the first stage
            var model = pipeline.Train<ProductData, ProductUnitPrediction>();

            //var prediction = model.Predict(new ProductData()
            //{
            //    ProductId = 818,
            //    Year = 2017,
            //    Month = 10,
            //    SalesCount = 2,
            //    MinQuantity = 2,
            //    MaxQuantity = 4,
            //    Quantity = 6,
            //    Prev = 131
            //});

            var prediction = model.Predict(new ProductData()
            {
                ProductId = productId,
                Year = year,
                Month = month,
                Max = max,
                Min = min,
                Count = count,
                Units = units,
                Avg = avg,
                Prev = prev
            });

            return new string[] { prediction.Score.ToString() };
        }
    }
}
