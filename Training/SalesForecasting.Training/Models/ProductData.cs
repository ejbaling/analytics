using Microsoft.ML.Runtime.Api;

namespace SalesForecasting.Training.Models
{
    /// <summary>
    /// Sample for sales quantity prediction by product model
    /// </summary>
    public class ProductData
    {
        [Column(ordinal: "0")]
        public float ProductId;

        [Column(ordinal: "1")]
        public float Year;

        [Column(ordinal: "2")]
        public float Month;

        [Column(ordinal: "3")]
        public float Count;

        [Column(ordinal: "4")]
        public float Min;

        [Column(ordinal: "5")]
        public float Max;

        [Column(ordinal: "6")]
        public float Units;

        [Column(ordinal: "7")]
        public float Avg;

        [Column(ordinal: "8")]
        public float Prev;

        [Column(ordinal: "9", name: "Label")]
        public float Next;
    }

    public class ProductUnitPrediction
    {
        public float Score;
    }
}
