using Meetup.ApplicationCore.Entities;

namespace MeetupAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SponsorsController : Controller
    {
        private readonly ISponsorService _sponsorService;
        private readonly IProducerService _producer;


        public SponsorsController(ISponsorService sponsorService, IProducerService producer)
        {
            _sponsorService = sponsorService;    
            _producer = producer;
        }

        /// <summary>
        /// Gets the list of Sponsors.
        /// </summary>
        /// <returns>Ok response containing Sponsors collection.</returns>
        /// <response code="200">Returns the list of Sponsors.</response>
        /// <response code="404">No Sponsors were found.</response>
        [HttpGet]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Sponsor>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSponsors()
        {
            var sponsors = await _sponsorService.GetAllAsync();

            // Uncomment if you want to use RabbitMQ
            //_producer.SendSponsorMessage(sponsors);

            return Ok(sponsors);
        }

        /// <summary>
        /// Gets Sponsor by it's ID.
        /// </summary>
        /// <param name="id">ID of the Sponsor to get.</param>
        /// <returns>Ok response containing a certain Sponsor.</returns>
        /// <response code="200">Returns a certain Sponsor.</response>
        /// <response code="404">No Sponsor was found.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSponsor([FromRoute]int id)
        { 
            var sponsor = await _sponsorService.GetByIdAsync(id);

            // Uncomment if you want to use RabbitMQ
            //_producer.SendSponsorMessage(sponsor);

            return Ok(sponsor);
        }

        /// <summary>
        /// Creates a new Sponsor.
        /// </summary>
        /// <param name="sponsorDto">The Sponsor to be created.</param>
        /// <returns>Ok response containing message.</returns>
        /// <response code="201">Sponsor is created.</response>
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateSponsor([FromBody] SponsorDto sponsorDto)
        {
            var sponsorToCreate = await _sponsorService.CreateAsync(sponsorDto);

            // Uncomment if you want to use RabbitMQ
            //_producer.SendSponsorMessage(sponsorToCreate);

            return Ok("Successfully created");
        }

        /// <summary>
        /// Updates a Sponsor with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the Sponsor to be updated.</param>
        /// <param name="updatedSponsor">The updated Sponsor data.</param>
        /// <returns>No Content response indicating the update was successful.</returns>
        /// <response code="204">The Sponsor was successfully updated.</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateSponsor([FromRoute]int id, 
                                                       [FromBody] SponsorDto updatedSponsor)
        { 
            var sponsorToUpdate = await _sponsorService.UpdateAsync(id, updatedSponsor);

            // Uncomment if you want to use RabbitMQ
            //_producer.SendSponsorMessage(sponsorToUpdate);

            return NoContent();
        }

        /// <summary>
        /// Removes a Sponsor with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the Sponsor to be removed.</param>
        /// <returns>No Content response indicating the removing was successful.</returns>
        /// <response code="204">The Sponsor was successfully removed.</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteSponsor([FromRoute] int id)
        {   
            var sponsorToDelete = await _sponsorService.DeleteAsync(id); 
            
            return NoContent();
        }
    }
}
