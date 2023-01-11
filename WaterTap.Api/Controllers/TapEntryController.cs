using Microsoft.AspNetCore.Mvc;
using WaterTap.Api.Models;
using WaterTap.Api.RepositoryServices;

namespace WaterTap.Api.Controllers
{
    [Route("api/tap-entries")]
    [ApiController]
    public class TapEntryController : ControllerBase
    {
        public readonly ITapEntryRepository _tapEntryRepository;

        public TapEntryController(ITapEntryRepository tapEntryRepository)
        {
            _tapEntryRepository = tapEntryRepository;
        }

        [HttpGet]
        public IActionResult GetTapEntries()
        {
            var taps = _tapEntryRepository.GetAllTapEntryDetails();

            return Ok(taps);
        }


        [HttpGet("{id:int}")]
        public IActionResult GetTapEntryDetail(int id)
        {
            var tapEntry = _tapEntryRepository.GetTapEntryDetail(id);

            return Ok(tapEntry);
        }

        [HttpPost]
        public IActionResult AddNewTapEntryDetail([FromBody] TapEntryDetailModel tapEntryDetailModel)
        {
            var tapId = _tapEntryRepository.AddNewTapEntryDetail(tapEntryDetailModel);

            return Ok(tapId);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTapEntryDetail(int id, [FromBody] TapEntryDetailModel tapEntryDetailModel)
        {
            var tapId = await _tapEntryRepository.UpdateTapEntryDetail(id, tapEntryDetailModel);

            return Ok(tapId);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteTapEntryDetail(int id)
        {
            _tapEntryRepository.DeleteTapEntryDetail(id);

            return Ok(id);
        }
    }
}
