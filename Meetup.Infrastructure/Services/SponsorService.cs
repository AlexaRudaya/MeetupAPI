namespace Meetup.Infrastructure.Services
{
    public sealed class SponsorService : ISponsorService
    {
        private readonly IValidator<SponsorDto> _validator;
        private readonly ISponsorRepository _sponsorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public SponsorService(IValidator<SponsorDto> validator, 
            ISponsorRepository sponsorRepository,
            IMapper mapper, ILogger logger)
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

            _logger.LogInformation("A sponsor is created successfully");

            return sponsor;
        }

        public async Task<SponsorDto> DeleteAsync(int sponsorId)
        {
            var sponsorToDelete = await _sponsorRepository.GetOneByAsync(expression: _ => _.Id.Equals(sponsorId));

            if (sponsorToDelete is null)
            {
                throw new SponsorNotFoundException($"Such sponsor with Id: {sponsorId} was not found");
            }

            await _sponsorRepository.DeleteAsync(sponsorToDelete!);

            _logger.LogInformation($"Sponsor with Id: {sponsorId} is removed");

            var sponsorDeleted = _mapper.Map<SponsorDto>(sponsorToDelete);

            return sponsorDeleted;
        }

        public async Task<SponsorDto> UpdateAsync(SponsorDto sponsor)
        {
            var validationResult = await _validator.ValidateAsync(sponsor);

            if (!validationResult.IsValid)
            {
                throw new InvalidValueException(validationResult.ToString());
            }

            var sponsorToUpdate = _mapper.Map<Sponsor>(sponsor);

            await _sponsorRepository.UpdateAsync(sponsorToUpdate);

            return sponsor;
        }
    }
}
