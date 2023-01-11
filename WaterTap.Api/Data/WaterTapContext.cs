using Microsoft.EntityFrameworkCore;

namespace WaterTap.Api.Data
{
    public class WaterTapContext : DbContext
    {

        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<TapInfo> TapInfo { get; set; }
        public DbSet<TapEntry> TapEntry { get; set; }
        public DbSet<MachineDetail> MachineDetail { get; set; }
    }
}
