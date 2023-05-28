namespace Meetup.Infrastructure.Services
{
    public sealed class EventService : IEventService
    {
        private readonly IValidator<EventDto> _validator;
        private readonly IEventRepository _eventRepository;
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ISpeakerRepository _speakerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EventService> _logger;

        public EventService(IValidator<EventDto> validator,
            IEventRepository eventRepository,
            ISponsorRepository sponsorRepository,
            ISpeakerRepository speakerRepository,
            IMapper mapper, ILogger<EventService> logger)
        {
            _validator = validator;
            _eventRepository = eventRepository;
            _sponsorRepository = sponsorRepository;
            _speakerRepository = speakerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets the collection of all Events.
        /// </summary>
        /// <returns>DTO representing the collection of all Events.</returns>
        /// <exception cref="EventNotFoundException">Thrown when no Events were found.</exception>
        public async Task<IEnumerable<EventDto>> GetAllAsync() 
        {
            var events = await _eventRepository.GetAllByAsync();  

            if (events is null)
            {
                throw new EventNotFoundException("No events were found");
            }

            _logger.LogInformation("Events are loaded");

            var eventsDto = _mapper.Map<IEnumerable<EventDto>>(events)
                                                              .OrderBy(_ => _.Date);

            return eventsDto;
        }

        /// <summary>
        /// Retrieves an Event by it's ID.
        /// </summary>
        /// <param name="eventId">ID of the Event wanted to get.</param>
        /// <returns>DTO of an Event with the given ID.</returns>
        /// <exception cref="EventNotFoundException">Thrown when there is no Event with such ID.</exception>
        public async Task<EventDto> GetByIdAsync(int eventId)
        {
            var entity = await _eventRepository.GetOneByAsync(expression: _ => _.Id.Equals(eventId));

            if (entity is null)
            {
                throw new EventNotFoundException($"Such event with Id: {eventId} was not found");
            }

            var eventDto = _mapper.Map<EventDto>(entity);

            return eventDto;
        }

        /// <summary>
        ///  Creates a new Event.
        /// </summary>
        /// <param name="eventDto">DTO for the Event to be created.</param>
        /// <returns>DTO for the ctreated Event.</returns>
        /// <exception cref="InvalidValueException">Thrown when the Event data fails validation.</exception>
        public async Task<EventDto> CreateAsync(EventDto eventDto)
        {
            var validationResult = await _validator.ValidateAsync(eventDto);

            if (!validationResult.IsValid)
            {
                throw new InvalidValueException(validationResult.ToString());
            }

            var eventToCreate = _mapper.Map<Event>(eventDto);

            eventToCreate.Sponsors!.Clear();
            eventToCreate.Speakers!.Clear();

            foreach (var sponsorId in eventDto.SponsorsIds)
            {
                var sponsor = await _sponsorRepository.GetOneManyToManyAsync(expression: _ => _.Id.Equals(sponsorId));

                if (sponsor is not null)
                {
                    eventToCreate.Sponsors!.Add(sponsor);
                }
            }

            foreach (var speakerId in eventDto.SpeakersIds)
            {
                var speaker = await _speakerRepository.GetOneManyToManyAsync(expression: _ => _.Id.Equals(speakerId));

                if (speaker is not null)
                {
                    eventToCreate.Speakers!.Add(speaker);
                }
            }

            await _eventRepository.CreateAsync(eventToCreate);

            _logger.LogInformation($"An event with Id:{eventToCreate.Id} is created successfully");

            return eventDto;
        }

        /// <summary>
        /// Updates an existing Event.
        /// </summary>
        /// <param name="id">ID of the Event to update.</param>
        /// <param name="eventDto">Updated Event DTO.</param>
        /// <returns>Updated Event DTO.</returns>
        /// <exception cref="InvalidValueException">Thrown when the Event data fails validation.</exception>
        /// <exception cref="EventNotFoundException">Thrown when there is no Event with such ID.</exception>
        public async Task<EventDto> UpdateAsync(int id, EventDto eventDto)  
        {
            var validationResult = await _validator.ValidateAsync(eventDto);

            if (!validationResult.IsValid)
            {
                throw new InvalidValueException(validationResult.ToString());
            }

            var existingEvent = await _eventRepository.GetOneByAsync(expression: _ => _.Id.Equals(id));

            if (existingEvent is null)
            {
                throw new EventNotFoundException($"Event with Id: {id} was not found");
            }

            existingEvent.Name = eventDto.Name;
            existingEvent.Description = eventDto.Description;
            existingEvent.Plan = eventDto.Plan;
            existingEvent.Date = eventDto.Date;
            existingEvent.Location = eventDto.Location;

            existingEvent.Sponsors!.Clear();
            existingEvent.Speakers!.Clear();

            foreach (var sponsorId in eventDto.SponsorsIds)
            {
                var sponsor = await _sponsorRepository.GetOneByAsync(expression: _ => _.Id.Equals(sponsorId));

                if (sponsor is not null)
                {
                    existingEvent.Sponsors!.Add(sponsor);
                }
            }

            foreach (var speakerId in eventDto.SpeakersIds)
            {
                var speaker = await _speakerRepository.GetOneByAsync(expression: _ => _.Id.Equals(speakerId));

                if (speaker is not null)
                {
                    existingEvent.Speakers!.Add(speaker);
                }
            }

            await _eventRepository.UpdateAsync(existingEvent);

            _logger.LogInformation($"Data for Event with Id: {existingEvent.Id} has been successfully updated.");

            return eventDto;
        }

        /// <summary>
        /// Removes an existing Event.
        /// </summary>
        /// <param name="eventId">ID of the Event to remove.</param>
        /// <returns>DTO representing the deleted Event.</returns>
        /// <exception cref="EventNotFoundException">Thrown when there is no Event with such ID.</exception>
        public async Task<EventDto> DeleteAsync(int eventId)
        {
            var eventToDelete = await _eventRepository.GetOneByAsync(expression: _ => _.Id.Equals(eventId));

            if (eventToDelete is null)
            {
                throw new EventNotFoundException($"Such event with Id: {eventId} was not found");
            }

            await _eventRepository.DeleteAsync(eventToDelete!);

            _logger.LogInformation($"Event with Id: {eventId} is removed");

            var eventDeleted = _mapper.Map<EventDto>(eventToDelete);

            return eventDeleted;
        }
    }
}
