using WaterTap.Api.Models;

namespace WaterTap.Api.RepositoryServices
{
    public interface IMachineRepository
    {
        List<MachineModel> GetAllMachines();
        List<MachineDetailModel> GetMachineDetail(Guid machineId, int month, int year);
        Guid AddNewMachineDeatil(MachineModel tapInfo);
        Task<Guid> UpdateMachineDetail(Guid id, MachineModel tapInfo);
        Task<Guid> DeleteMachineDetail(Guid id);
    }
}
