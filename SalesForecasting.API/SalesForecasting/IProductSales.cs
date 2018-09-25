using System.Threading.Tasks;

namespace Analytics.Model.SalesForecasting
{
    public interface IProductSales
    {
        Task<ProductUnitPrediction> Predict(string modelPath, string productId, float year, float month, float units, float avg, float count, float max, float min, float prev);
    }
}
