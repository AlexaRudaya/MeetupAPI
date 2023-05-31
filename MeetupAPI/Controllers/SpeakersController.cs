namespace MeetupAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SpeakersController : ControllerBase
    {
        private readonly ISpeakerService _speakerService;
        private readonly IProducerService _producer;

        public SpeakersController(ISpeakerService speakerService, IProducerService producer)
        {
            _speakerService = speakerService;
            _producer = producer;
        }

        /// <summary>
        /// Gets the list of Speakers.
        /// </summary>
        /// <returns>Ok response containing Speakers collection.</returns>
        /// <response code="200">Returns the list of Speakers.</response>
        /// <response code="404">No Speakers were found.</response>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Speaker>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSpeakers()
        {
            var speakers = await _speakerService.GetAllAsync();

            _producer.SendSpeakerMessage(speakers);

            return Ok(speakers);
        }

        /// <summary>
        /// Gets Speaker by it's ID.
        /// </summary>
        /// <param name="id">ID of the Speaker to get.</param>
        /// <returns>Ok response containing a certain Speaker.</returns>
        /// <response code="200">Returns a certain Speaker.</response>
        /// <response code="404">No Speaker was found.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSpeaker([FromRoute] int id)
        {
            var speaker = await _speakerService.GetByIdAsync(id);

            _producer.SendSpeakerMessage(speaker);

            return Ok(speaker);
        }

        /// <summary>
        /// Creates a new Speaker.
        /// </summary>
        /// <param name="speakerDto">The Speaker to be created.</param>
        /// <returns>Ok response containing message.</returns>
        /// <response code="201">Speaker is created.</response>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateSpeaker([FromBody] SpeakerDto speakerDto)
        {
            var speakerToCreate = await _speakerService.CreateAsync(speakerDto);

            _producer.SendSpeakerMessage(speakerToCreate);

            return Ok("Successfully created");
        }

        /// <summary>
        /// Updates a Speaker with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the Speaker to be updated.</param>
        /// <param name="updatedSpeaker">The updated Speaker data.</param>
        /// <returns>No Content response indicating the update was successful.</returns>
        /// <response code="204">The Speaker was successfully updated.</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateSpeaker([FromRoute] int id,
                                                       [FromBody] SpeakerDto updatedSpeaker)
        {
            var speakerToUpdate = await _speakerService.UpdateAsync(id, updatedSpeaker);

            _producer.SendSpeakerMessage(speakerToUpdate);

            return NoContent();
        }

        /// <summary>
        /// Removes a Speaker with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the Speaker to be removed.</param>
        /// <returns>No Content response indicating the removing was successful.</returns>
        /// <response code="204">The Speaker was successfully removed.</response>
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
