namespace Meetup.ApplicationCore.Services
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

        /// <summary>
        /// Gets the collection of all Speakers.
        /// </summary>
        /// <returns>DTO representing the collection of all Speakers.</returns>
        /// <exception cref="SpeakerNotFoundException">Thrown when no Speakers were found.</exception>
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

        /// <summary>
        /// Retrieves a Speaker by it's ID.
        /// </summary>
        /// <param name="speakerId">ID of the Speaker wanted to get.</param>
        /// <returns>DTO of a Speaker with the given ID.</returns>
        /// <exception cref="SpeakerNotFoundException">Thrown when there is no Speaker with such ID.</exception>
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

        /// <summary>
        ///  Creates a new Speaker.
        /// </summary>
        /// <param name="speaker">DTO for the Speaker to be created.</param>
        /// <returns>DTO for the ctreated Speaker.</returns>
        /// <exception cref="InvalidValueException">Thrown when the Speaker data fails validation.</exception>
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

        /// <summary>
        /// Updates an existing Speaker.
        /// </summary>
        /// <param name="id">ID of the Speaker to update.</param>
        /// <param name="speaker">Updated Speaker DTO.</param>
        /// <returns>Updated Speaker DTO.</returns>
        /// <exception cref="InvalidValueException">Thrown when the Speaker data fails validation.</exception>
        /// <exception cref="SpeakerNotFoundException">Thrown when there is no Speaker with such ID.</exception>
        public async Task<SpeakerDto> UpdateAsync(int id, SpeakerDto speaker)
        {
            var validationResult = await _validator.ValidateAsync(speaker);

            if (!validationResult.IsValid)
            {
                throw new InvalidValueException(validationResult.ToString());
            }

            var existingSpeaker = await _speakerRepository.GetOneByAsync(expression: _ => _.Id.Equals(id));

            if (existingSpeaker.Id != id)
            {
                _logger.LogError($"Failed finding speaker with Id:{id} while updating data.");
                throw new SpeakerNotFoundException($"Such speaker with Id: {id} was not found");
            }

            existingSpeaker.Name = speaker.Name;

            await _speakerRepository.UpdateAsync(existingSpeaker);

            _logger.LogInformation($"Data for Speaker with Id: {existingSpeaker.Id} has been successfully updated.");

            return speaker;
        }

        /// <summary>
        /// Removes an existing Speaker.
        /// </summary>
        /// <param name="speakerId">ID of the Speaker to remove.</param>
        /// <returns>DTO representing the deleted Speaker.</returns>
        /// <exception cref="SpeakerNotFoundException">Thrown when there is no Speaker with such ID.</exception>
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