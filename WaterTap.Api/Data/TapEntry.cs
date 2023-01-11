namespace WaterTap.Api.Data
{
    public class TapEntry
    {
        public int Id { get; set; }
        public int TapId { get; set; }
        public string TapName { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public double Amount { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}
