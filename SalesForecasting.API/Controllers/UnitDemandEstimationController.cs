using Analytics.Model.SalesForecasting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> GetAsync(
            [FromQuery]string productId,
            [FromQuery]int year, 
            [FromQuery]int month,
            [FromQuery]float units, 
            [FromQuery]float avg,
            [FromQuery]int count, 
            [FromQuery]float max,
            [FromQuery]float min, 
            [FromQuery]float prev)
        {
            // next,productId,year,month,units,avg,count,max,min,prev
            var nextMonthUnitDemandEstimation = await this.productSales.Predict($"ModelsAI/product_month_fastTreeTweedie.zip", productId, year, month, units, avg, count, max, min, prev);

            return Ok(nextMonthUnitDemandEstimation.Score);
        }
    }
}