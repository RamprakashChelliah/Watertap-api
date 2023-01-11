using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WaterTap.Api.Models;
using WaterTap.Api.RepositoryServices;

namespace WaterTap.Api.Controllers
{
    [Route("api/machines")]
    [ApiController]
    public class MachineController : ControllerBase
    {
        public readonly IMachineRepository _machineRepository;

        public MachineController(IMachineRepository tapEntryRepository)
        {
            _machineRepository = tapEntryRepository;
        }

        [HttpGet]
        public IActionResult GetMachines()
        {
            var machines = _machineRepository.GetAllMachines();

            return Ok(machines);
        }


        [HttpGet("{id:guid}/{month:int}/{year:int}")]
        public IActionResult GetTapEntryDetail(Guid id, int month, int year)
        {
            var machineDetail = _machineRepository.GetMachineDetail(id, month, year);

            return Ok(machineDetail);
        }

        [HttpPost]
        public IActionResult AddNewMachineDetail([FromBody] MachineModel machineDetailModel)
        {
            var machineId = _machineRepository.AddNewMachineDeatil(machineDetailModel);

            return Ok(machineId);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateMachineDetail(Guid id, [FromBody] MachineModel machineDetailModel)
        {
            var machineId = await _machineRepository.UpdateMachineDetail(id, machineDetailModel);

            return Ok(machineId);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteMachineDetail(Guid id)
        {
            _machineRepository.DeleteMachineDetail(id);

            return Ok(id);
        }
    }
}
