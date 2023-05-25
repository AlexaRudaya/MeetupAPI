namespace Meetup.Infrastructure.Services
{
    public sealed class SpeakerService : ISpeakerService
    {
        private readonly IValidator<SpeakerDto> _validator;
        private readonly ISpeakerRepository _speakerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public SpeakerService(IValidator<SpeakerDto> validator,
            ISpeakerRepository speakerRepository,
            IMapper mapper, ILogger logger)
        {
            _validator = validator;
            _speakerRepository = speakerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<SpeakerDto>> GetAllAsync()
        {
            var speakers = await _speakerRepository.GetAllByAsync();

            if (speakers is null)
            {
                throw new SpeakerNotFoundException("No speakers were found");
            }

            _logger.LogInformation("Speakers are loaded");

            var speakersDto = _mapper.Map<IEnumerable<SpeakerDto>>(speakers)
                                                                   .OrderBy(_ => _.Name);

            return speakersDto;
        }

        public async Task<SpeakerDto> GetByIdAsync(int speakerId)
        {
            var entity = await _speakerRepository.GetOneByAsync(expression: _ => _.Id.Equals(speakerId));

            if (entity is null)
            {
                throw new SpeakerNotFoundException($"Such speaker with Id: {speakerId} was not found");
            }

            var speakerDto = _mapper.Map<SpeakerDto>(entity);

            return speakerDto;
        }

        public async Task<SpeakerDto> CreateAsync(SpeakerDto speaker)
        {
            var validationResult = await _validator.ValidateAsync(speaker);

            if (!validationResult.IsValid)
            {
                throw new InvalidValueException(validationResult.ToString());
            }

            var speakerToCreate = _mapper.Map<Speaker>(speaker);

            await _speakerRepository.CreateAsync(speakerToCreate);

            _logger.LogInformation("A speaker is created successfully");

            return speaker;
        }

        public async Task<SpeakerDto> DeleteAsync(int speakerId)
        {
            var speakerToDelete = await _speakerRepository.GetOneByAsync(expression: _ => _.Id.Equals(speakerId));

            if (speakerToDelete is null)
            {
                throw new SpeakerNotFoundException($"Such speaker with Id: {speakerId} was not found");
            }

            await _speakerRepository.DeleteAsync(speakerToDelete!);

            _logger.LogInformation($"Speaker with Id: {speakerId} is removed");

            var speakerDeleted = _mapper.Map<SpeakerDto>(speakerToDelete);

            return speakerDeleted;
        }

        public async Task<SpeakerDto> UpdateAsync(SpeakerDto speaker)
        {
            var validationResult = await _validator.ValidateAsync(speaker);

            if (!validationResult.IsValid)
            {
                throw new InvalidValueException(validationResult.ToString());
            }

            var speakerToUpdate = _mapper.Map<Speaker>(speaker);

            await _speakerRepository.UpdateAsync(speakerToUpdate);

            return speaker;
        }
    }
}
