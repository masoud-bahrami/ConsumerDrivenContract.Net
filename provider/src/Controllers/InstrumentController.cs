using IranianMusic.Instruments.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace IranianMusic.Instruments.Provider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstrumentController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllSync()
        {
            return new JsonResult(
                new
                {
                    body = new
                    {
                        result = "Success",
                        message = "",
                        data = IranianMusicIstrumentsRepository.GetAllFormal()
                    },
                    status = 200
                });
        }

        [HttpGet("{name}")]
        public IActionResult GetSync(string name)
        {
            if (IranianMusicIstrumentsRepository.HasFormalInstrument(name))
                return new JsonResult(
                    new
                    {
                        result = "Success",
                        data = IranianMusicIstrumentsRepository.GetFormalInstrumentDescription(name)
                    });

            if (IranianMusicIstrumentsRepository.HasSecoundaryInstrument(name))
                return BadRequest(new
                {
                    message = $"{name} is a secoundary Instrument",
                });

            return NotFound();
        }
    }
}