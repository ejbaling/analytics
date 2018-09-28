using Analytics.Model.SalesForecasting;
using Microsoft.AspNetCore.Mvc;

namespace SalesForecasting.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitDemandEstimationController : ControllerBase
    {
        private readonly IProductSales productSales;

        public UnitDemandEstimationController(IProductSales productSales)
        {
            this.productSales = productSales;
        }

        // GET api/unitdemandestimation
        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> GetAsync(ProductData[] data)
        {

            foreach(var d in data)
            {
                var prediction = await this.productSales.Predict($"ModelsAI/product_month_fastTreeTweedie.zip", d.ProductId, d.Year, d.Month, d.Units, d.Avg, d.Count, d.Max, d.Min, d.Prev);
                d.Next = prediction.Score;
            }

            return new JsonResult(data);
        }
    }
}