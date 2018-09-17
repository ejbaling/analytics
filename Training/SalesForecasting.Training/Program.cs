using System;
using System.Threading.Tasks;

namespace SalesForecasting.Training
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                await ProductModelHelper.SaveModel("data\\products.stats.csv");
                await ProductModelHelper.TestPrediction();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Console.ReadLine();
            }

            Console.ReadLine();
        }
    }
}
