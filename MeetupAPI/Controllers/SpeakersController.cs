namespace MeetupAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeakersController : ControllerBase
    {
        private readonly ISpeakerService _speakerService;

        public SpeakersController(ISpeakerService speakerService)
        {
            _speakerService = speakerService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Speaker>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSpeakers()
        {
            var speakers = await _speakerService.GetAllAsync();

            return Ok(speakers);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSpeaker([FromRoute] int id)
        {
            var speaker = await _speakerService.GetByIdAsync(id);

            return Ok(speaker);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateSpeaker([FromBody] SpeakerDto speakerDto)
        {
            var speakerToCreate = await _speakerService.CreateAsync(speakerDto);

            return Ok("Successfully created");
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateSpeaker([FromRoute] int id,
                                                       [FromBody] SpeakerDto updatedSpeaker)
        {
            var speakerToUpdate = await _speakerService.UpdateAsync(id, updatedSpeaker);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteSpeakerr([FromRoute] int id)
        {
            var speakerToDelete = await _speakerService.DeleteAsync(id);

            return NoContent();
        }
    }
}
