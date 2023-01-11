namespace WaterTap.Api.Models
{
    public class TapEntryDetailModel
    {
        public int Id { get; set; }
        public int TapId { get; set; }
        public string TapName { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public double Amount { get; set; }
    }
}
