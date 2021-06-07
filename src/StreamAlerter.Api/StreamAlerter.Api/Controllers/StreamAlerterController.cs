using Microsoft.AspNetCore.Mvc;
using StreamAlerter.Core.Interfaces.Business;
using StreamAlerter.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StreamAlerter.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StreamAlerterController : ControllerBase
    {
        private IStreamAlerterService streamAlerterService;

        public StreamAlerterController(IStreamAlerterService streamAlerterService)
        {
            this.streamAlerterService = streamAlerterService;
        }

        [HttpPut]
        public IActionResult Put(Entities.Streamer streamer)
        {
            if (streamer == null || string.IsNullOrEmpty(streamer.Name))
                return BadRequest("Please enter a name in the request");
            streamer.Name = streamer.Name.ToLower();
            var streamerUpserted = streamAlerterService.Create(streamer);
            return Ok(streamerUpserted);
        }

        [HttpPost]
        public IActionResult Post(Entities.Streamer streamer)
        {
            if (streamer == null || string.IsNullOrEmpty(streamer.Name))
                return BadRequest("Please enter a name in the request");
            streamer.Name = streamer.Name.ToLower();
            var streamerUpserted = streamAlerterService.Upsert(streamer);
            return Ok(streamerUpserted);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Entities.Streamer> streamers = streamAlerterService.RetrieveAll();
            return Ok(streamers);
        }

        [HttpDelete]
        [Route("{name}")]
        public IActionResult Delete(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest("Please enter a name in the request");
            name = name.ToLower();
            var isDeleted = streamAlerterService.Remove(name);
            return Ok(isDeleted);
        }

        [HttpGet]
        [Route("getLiveStreamers")]
        public async Task<IActionResult> GetLive()
        {
            IEnumerable<Streamer> streamer = await streamAlerterService.GetLiveStreamers();
            return Ok(streamer);
        }
    }
}
