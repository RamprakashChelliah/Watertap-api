namespace WaterTap.Api.Data
{
    public class MachineEntry
    {
        public DateTime Date { get; set; }
        public double TimeDuration { get; set; }
        public List<PlayHistory> PlayHistories { get; set; } = new List<PlayHistory>();
    }
}
