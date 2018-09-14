using Microsoft.ML.Runtime.Api;

namespace Microsoft.eShopOnContainers.Services.AI.SalesForecasting.Training.MLNet.API
{
    /// <summary>
    /// Sample for sales prediction by product model
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
        public float SalesCount;

        [Column(ordinal: "4")]
        public float MinQuantity;

        [Column(ordinal: "5")]
        public float MaxQuantity;

        [Column(ordinal: "6")]
        public float Quantity;

        [Column(ordinal: "7")]
        public float Prev;

        [Column(ordinal: "8", name: "Label")]
        public float Next;
    }

    public class ProductUnitPrediction
    {
        public float Score;
    }
}
