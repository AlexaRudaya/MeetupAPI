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

        public async Task<SponsorDto> UpdateAsync(int id,SponsorDto sponsor)
        {
            var validationResult = await _validator.ValidateAsync(sponsor);

            if (!validationResult.IsValid)
            {
                throw new InvalidValueException(validationResult.ToString());
            }

            var existingSponsor = await _sponsorRepository.GetOneByAsync(expression: _ => _.Id.Equals(id));

            if (!existingSponsor.Id.Equals(id))
            {
                _logger.LogError($"Failed finding sponsor with Id:{id} while updating data.");
                throw new SponsorNotFoundException($"Such sponsor with Id: {id} was not found");
            }

            var sponsorToUpdate = _mapper.Map<Sponsor>(sponsor);

            await _sponsorRepository.UpdateAsync(sponsorToUpdate);

            _logger.LogInformation($"Data for Sponsor with Id: {sponsor.Id} has been successfully updated.");

            return sponsor;
        }

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
