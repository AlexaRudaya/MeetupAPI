namespace MeetupAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Gets the list of Events.
        /// </summary>
        /// <returns>Ok response containing Events collection.</returns>
        /// <response code="200">Returns the list of Events.</response>
        /// <response code="404">No Events were found.</response>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Event>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _eventService.GetAllAsync();

            return Ok(events);
        }

        /// <summary>
        /// Gets Event by it's ID.
        /// </summary>
        /// <param name="id">ID of the Event to get.</param>
        /// <returns>Ok response containing a certain Event.</returns>
        /// <response code="200">Returns a certain Event.</response>
        /// <response code="404">No Event was found.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEvent(int id)
        { 
            var oneEvent = await _eventService.GetByIdAsync(id);

            return Ok(oneEvent);
        }

        /// <summary>
        /// Creates a new Event.
        /// </summary>
        /// <param name="eventDto">The Event to be created.</param>
        /// <returns>Ok response containing message.</returns>
        /// <response code="201">Event is created.</response>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateEvent([FromBody] EventDto eventDto)
        {
            var eventToCreate = await _eventService.CreateAsync(eventDto);

            return Ok("Successfully created");
        }

        /// <summary>
        /// Updates an Event with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the Event to be updated.</param>
        /// <param name="updatedEvent">The updated Event data.</param>
        /// <returns>No Content response indicating the update was successful.</returns>
        /// <response code="204">The Event was successfully updated.</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateEvent([FromRoute] int id,
                                                     [FromBody] EventDto updatedEvent)
        {
            var eventToUpdate = await _eventService.UpdateAsync(id, updatedEvent);

            return NoContent();
        }

        /// <summary>
        /// Removes an Event with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the Event to be removed.</param>
        /// <returns>No Content response indicating the removing was successful.</returns>
        /// <response code="204">The Event was successfully removed.</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteEvent([FromRoute] int id)
        {
            var eventToDelete = await _eventService.DeleteAsync(id);

            return NoContent();
        }
    }
}
