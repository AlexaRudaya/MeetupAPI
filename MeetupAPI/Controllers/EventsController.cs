namespace MeetupAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Event>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _eventService.GetAllAsync();

            return Ok(events);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEvent(int id)
        { 
            var oneEvent = await _eventService.GetByIdAsync(id);

            return Ok(oneEvent);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateEvent([FromBody] EventDto eventDto)
        {
            var eventToCreate = await _eventService.CreateAsync(eventDto);

            return Ok("Successfully created");
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateEvent([FromRoute] int id,
                                                      [FromBody] EventDto updatedEvent)
        {
            var eventToUpdate = await _eventService.UpdateAsync(id, updatedEvent);

            return NoContent();
        }

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
