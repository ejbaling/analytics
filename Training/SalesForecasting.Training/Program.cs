using System;

namespace SalesForecasting.Training
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //await ProductModelHelper.SaveModel("data/products.stats.csv");
                //await ProductModelHelper.TestPrediction();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
