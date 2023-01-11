namespace WaterTap.Api.Data
{
    public class MachineDetail
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<MachineEntry> MachineEntries { get; set; } = new List<MachineEntry>();
        public DateTime InsertedDate { get; set; }

    }
}
