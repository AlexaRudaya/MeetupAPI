namespace Meetup.Infrastructure.Services
{
    public sealed class SpeakerService : ISpeakerService
    {
        private readonly IValidator<SpeakerDto> _validator;
        private readonly ISpeakerRepository _speakerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SpeakerService> _logger;

        public SpeakerService(IValidator<SpeakerDto> validator,
            ISpeakerRepository speakerRepository,
            IMapper mapper, ILogger<SpeakerService> logger)
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
                _logger.LogError("Failed loading speakers list.");
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
                _logger.LogError($"Failed finding speaker with Id:{speakerId}.");
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

            _logger.LogInformation($"A speaker with Id:{speakerToCreate.Id} and Name:{speakerToCreate.Name} is created successfully");

            return speaker;
        }

        public async Task<SpeakerDto> UpdateAsync(int id, SpeakerDto speaker)
        {
            var validationResult = await _validator.ValidateAsync(speaker);

            if (!validationResult.IsValid)
            {
                throw new InvalidValueException(validationResult.ToString());
            }

            var existingSpeaker = await _speakerRepository.GetOneByAsync(expression: _ => _.Id.Equals(id));

            if (!existingSpeaker.Id.Equals(id))
            {
                _logger.LogError($"Failed finding speaker with Id:{id} while updating data.");
                throw new SpeakerNotFoundException($"Such speaker with Id: {id} was not found");
            }

            var speakerToUpdate = _mapper.Map<Speaker>(speaker);

            await _speakerRepository.UpdateAsync(speakerToUpdate);

            _logger.LogInformation($"Data for Speaker with Id: {speaker.Id} has been successfully updated.");

            return speaker;
        }

        public async Task<SpeakerDto> DeleteAsync(int speakerId)
        {
            var speakerToDelete = await _speakerRepository.GetOneByAsync(expression: _ => _.Id.Equals(speakerId));

            if (speakerToDelete is null || !speakerToDelete.Id.Equals(speakerId))
            {
                _logger.LogError($"Failed finding speaker with Id:{speakerId} while deleting entity.");
                throw new SpeakerNotFoundException($"Such speaker with Id: {speakerId} was not found");
            }

            var speakerDeleted = _mapper.Map<SpeakerDto>(speakerToDelete);

            await _speakerRepository.DeleteAsync(speakerToDelete!);

            _logger.LogInformation($"Speaker with Id: {speakerId} is removed");

            return speakerDeleted;
        }
    }
}
