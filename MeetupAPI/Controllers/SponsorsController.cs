namespace MeetupAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SponsorsController : Controller
    {
        private readonly ISponsorService _sponsorService;

        public SponsorsController(ISponsorService sponsorService)
        {
            _sponsorService = sponsorService;    
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Sponsor>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSponsors()
        {
            var sponsors = await _sponsorService.GetAllAsync();

            return Ok(sponsors);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSponsor([FromRoute]int id)
        { 
            var sponsor = await _sponsorService.GetByIdAsync(id);

            return Ok(sponsor);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateSponsor([FromBody] SponsorDto sponsorDto)
        {
            var sponsorToCreate = await _sponsorService.CreateAsync(sponsorDto);

            return Ok("Successfully created");
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateSponsor([FromRoute]int id, 
                                                       [FromBody] SponsorDto updatedSponsor)
        { 
            var sponsorToUpdate = await _sponsorService.UpdateAsync(id, updatedSponsor);

            return NoContent();
        }

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
