namespace Meetup.Infrastructure.Services
{
    public sealed class EventService : IEventService
    {
        private readonly IValidator<EventDto> _validator;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EventService> _logger;

        public EventService(IValidator<EventDto> validator,
            IEventRepository eventRepository,
            IMapper mapper, ILogger<EventService> logger)
        {
            _validator = validator;
            _eventRepository = eventRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<EventDto>> GetAllAsync()
        {
            var events = await _eventRepository.GetAllByAsync(
                include: _ => _
                    .Include(_ => _.Sponsors!)
                    .Include(_ => _.Speakers!));

            if (events is null)
            {
                throw new EventNotFoundException("No events were found");
            }

            _logger.LogInformation("Events are loaded");

            var eventsDto = _mapper.Map<IEnumerable<EventDto>>(events)
                                                              .OrderBy(_ => _.Date);

            return eventsDto;
        }

        public async Task<EventDto> GetByIdAsync(int eventId)
        {
            var entity = await _eventRepository.GetOneByAsync(
                include: _ => _
                    .Include(_ => _.Sponsors!)
                    .Include(_ => _.Speakers!),
                expression: _ => _.Id.Equals(eventId));

            if (entity is null)
            {
                throw new EventNotFoundException($"Such event with Id: {eventId} was not found");
            }

            var eventDto = _mapper.Map<EventDto>(entity);

            return eventDto;
        }

        public async Task<EventDto> CreateAsync(EventDto eventDto)
        {
            var validationResult = await _validator.ValidateAsync(eventDto);

            if (!validationResult.IsValid)
            {
                throw new InvalidValueException(validationResult.ToString());
            }

            var eventToCreate = _mapper.Map<Event>(eventDto);

            await _eventRepository.CreateAsync(eventToCreate);

            _logger.LogInformation("An event is created successfully");

            return eventDto;
        }

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

        public async Task<EventDto> UpdateAsync(int id, EventDto eventDto)
        {
            var validationResult = await _validator.ValidateAsync(eventDto);

            if (!validationResult.IsValid)
            {
                throw new InvalidValueException(validationResult.ToString());
            }

            var eventToUpdate = _mapper.Map<Event>(eventDto);

            var eventEntity = await _eventRepository.GetOneByAsync(
                include: _ => _
                    .Include(_ => _.Sponsors!)
                    .Include(_ => _.Speakers!),
                expression: _ => _.Id.Equals(eventToUpdate.Id));

            if (eventEntity is null)
            {
                throw new EventNotFoundException($"Event with Id: {eventToUpdate.Id} was not found");
            }

            _mapper.Map(eventToUpdate, eventEntity);

            await _eventRepository.UpdateAsync(eventEntity);

            var updatedEventDto = _mapper.Map<EventDto>(eventEntity);

            return eventDto;
        }
    }
}
