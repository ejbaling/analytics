using Microsoft.ML.Runtime.Api;

namespace Analytics.Model.SalesForecasting
{
    /// <summary>
    /// Sample for sales quantity prediction by product model
    /// </summary>
    public class ProductData
    {
        public ProductData()
        {
            
        }

        public ProductData(string productId, int year, int month, float units, float avg,
            int count, float max, float min, float prev)
        {
            this.ProductId = productId;
            this.Year = year;
            this.Month = month;
            this.Units = units;
            this.Avg = avg;
            this.Count = count;
            this.Max = max;
            this.Min = min;
            this.Prev = prev;
        }

        [Column(ordinal: "0", name: "Label")]
        public float Next;

        [Column(ordinal: "1")]
        public string ProductId;

        [Column(ordinal: "2")]
        public float Year;

        [Column(ordinal: "3")]
        public float Month;

        [Column(ordinal: "4")]
        public float Units;

        [Column(ordinal: "5")]
        public float Avg;

        [Column(ordinal: "6")]
        public float Count;

        [Column(ordinal: "7")]
        public float Max;

        [Column(ordinal: "8")]
        public float Min;

        [Column(ordinal: "9")]
        public float Prev;

        
    }

    public class ProductUnitPrediction
    {
        public float Score;
    }
}
