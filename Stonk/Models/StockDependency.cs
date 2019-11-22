namespace Stonks.Controllers
{
    internal class StockDependency
    {
        public int ID { get; set; }
        public int SourceID { get; set; }

        public int TargetID { get; set; }
        public double multiplier { get; set; }

    }
}