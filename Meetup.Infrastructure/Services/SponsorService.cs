namespace Meetup.Infrastructure.Services
{
    public sealed class SponsorService : ISponsorService
    {
        private readonly IValidator<SponsorDto> _validator;
        private readonly ISponsorRepository _sponsorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SponsorService> _logger;

        public SponsorService(IValidator<SponsorDto> validator, 
            ISponsorRepository sponsorRepository,
            IMapper mapper, ILogger<SponsorService> logger)
        {
            _validator = validator;
            _sponsorRepository = sponsorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets the collection of all Sponsors.
        /// </summary>
        /// <returns>DTO representing the collection of all Sponsors.</returns>
        /// <exception cref="SponsorNotFoundException">Thrown when no Sponsors were found.</exception>
        public async Task<IEnumerable<SponsorDto>> GetAllAsync()
        {
            var sponsors = await _sponsorRepository.GetAllByAsync();

            if (sponsors is null) 
            {
                _logger.LogError("Failed loading sponsors list.");
                throw new SponsorNotFoundException("No sponsors were found");       
            }

            _logger.LogInformation("Sponsors are loaded");

            var sponsorsDto = _mapper.Map<IEnumerable<SponsorDto>>(sponsors)
                                                                   .OrderBy(_ => _.Name);

            return sponsorsDto;
        }

        /// <summary>
        /// Retrieves a Sponsor by it's ID.
        /// </summary>
        /// <param name="sponsorId">ID of the Sponsor wanted to get.</param>
        /// <returns>DTO of a Sponsor with the given ID.</returns>
        /// <exception cref="SponsorNotFoundException">Thrown when there is no Sponsor with such ID.</exception>
        public async Task<SponsorDto> GetByIdAsync(int sponsorId)
        {
            var entity = await _sponsorRepository.GetOneByAsync(expression: _ => _.Id.Equals(sponsorId));

            if (entity is null)
            {
                _logger.LogError($"Failed finding sponsor with Id:{sponsorId}.");
                throw new SponsorNotFoundException($"Such sponsor with Id: {sponsorId} was not found");
            }

            var sponsorDto = _mapper.Map<SponsorDto>(entity);

            return sponsorDto;
        }

        /// <summary>
        /// Creates a new Sponsor.
        /// </summary>
        /// <param name="sponsor">DTO for the Sponsor to be created.</param>
        /// <returns>DTO for the ctreated Sponsor.</returns>
        /// <exception cref="InvalidValueException">Thrown when the Sponsor data fails validation.</exception>
        public async Task<SponsorDto> CreateAsync(SponsorDto sponsor)
        {
            var validationResult = await _validator.ValidateAsync(sponsor);

            if (!validationResult.IsValid)
            {
                throw new InvalidValueException(validationResult.ToString());
            }

            var sponsorToCreate = _mapper.Map<Sponsor>(sponsor);

            await _sponsorRepository.CreateAsync(sponsorToCreate);

            _logger.LogInformation($"A sponsor with Id:{sponsorToCreate.Id} and Name:{sponsorToCreate.Name} is created successfully");

            return sponsor;
        }

        /// <summary>
        /// Updates an existing Sponsor.
        /// </summary>
        /// <param name="id">ID of the Sponsor to update.</param>
        /// <param name="sponsor">Updated Sponsor DTO.</param>
        /// <returns>Updated Sponsor DTO.</returns>
        /// <exception cref="InvalidValueException">Thrown when the Sponsor data fails validation.</exception>
        /// <exception cref="SponsorNotFoundException">Thrown when there is no Sponsor with such ID.</exception>
        public async Task<SponsorDto> UpdateAsync(int id,SponsorDto sponsor)
        {
            var validationResult = await _validator.ValidateAsync(sponsor);

            if (!validationResult.IsValid)
            {
                throw new InvalidValueException(validationResult.ToString());
            }

            var existingSponsor = await _sponsorRepository.GetOneByAsync(expression: _ => _.Id.Equals(id));

            if (existingSponsor.Id != id)
            {
                _logger.LogError($"Failed finding sponsor with Id:{id} while updating data.");
                throw new SponsorNotFoundException($"Such sponsor with Id: {id} was not found");
            }

            existingSponsor.Name = sponsor.Name;

            await _sponsorRepository.UpdateAsync(existingSponsor);

            _logger.LogInformation($"Data for Sponsor with Id: {existingSponsor.Id} has been successfully updated.");

            return sponsor;
        }

        /// <summary>
        /// Removes an existing Sponsor.
        /// </summary>
        /// <param name="sponsorId">ID of the Sponsor to remove.</param>
        /// <returns>DTO representing the deleted Sponsor.</returns>
        /// <exception cref="SponsorNotFoundException">Thrown when there is no Sponsor with such ID.</exception>
        public async Task<SponsorDto> DeleteAsync(int sponsorId)
        {
            var sponsorToDelete = await _sponsorRepository.GetOneByAsync(expression: _ => _.Id.Equals(sponsorId));

            if (sponsorToDelete is null || !sponsorToDelete.Id.Equals(sponsorId))
            {
                _logger.LogError($"Failed finding sponsor with Id:{sponsorId} while deleting entity.");
                throw new SponsorNotFoundException($"Such sponsor with Id: {sponsorId} was not found");
            }

            var sponsorDeleted = _mapper.Map<SponsorDto>(sponsorToDelete);

            await _sponsorRepository.DeleteAsync(sponsorToDelete!);

            _logger.LogInformation($"Sponsor with Id: {sponsorId} is removed");

            return sponsorDeleted;
        }
    }
}
