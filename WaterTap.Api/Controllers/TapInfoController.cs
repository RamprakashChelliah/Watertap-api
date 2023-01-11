using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WaterTap.Api.Models;
using WaterTap.Api.RepositoryServices;

namespace WaterTap.Api.Controllers
{
    [Route("api/tap-details")]
    [ApiController]
    public class TapInfoController : ControllerBase
    {
        public readonly ITapDetailRepository _tapDetailRepository;

        public TapInfoController(ITapDetailRepository tapDetailRepository)
        {
            _tapDetailRepository = tapDetailRepository;
        }

        [HttpGet]
        public IActionResult GetTaps()
        {
            var taps = _tapDetailRepository.GetAllTapDetails();

            return Ok(taps);
        }


        [HttpGet("{id:int}")]
        public IActionResult GetTapDetail(int id)
        {
            var tapDetail = _tapDetailRepository.GetTapDetail(id);

            return Ok(tapDetail);
        }

        [HttpPost]
        public IActionResult AddNewTapDetail([FromBody] TapDetailModel tapDetailModel)
        {
            var tapId = _tapDetailRepository.AddNewTapDetail(tapDetailModel);

            return Ok(tapId);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTapDetail(int id, [FromBody] TapDetailModel tapDetailModel)
        {
            var tapId = await _tapDetailRepository.UpdateTapDetail(id, tapDetailModel);

            return Ok(tapId);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteTapDetail(int id)
        {
            _tapDetailRepository.DeleteTapDetail(id);

            return Ok(id);
        }
    }
}
