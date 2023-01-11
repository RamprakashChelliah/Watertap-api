namespace WaterTap.Api.Models
{
    public class MachineDetailModel
    {
        public Guid MachineId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double TimeDuration { get; set; }
    }
}
