namespace MeetupAPI.Tests.Services
{
    public class EventServiceTests
    {
        private readonly IValidator<EventDto> _validator;
        private readonly IEventRepository _eventRepository;
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ISpeakerRepository _speakerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EventService> _logger;

        public EventServiceTests()
        {
            _validator = A.Fake<IValidator<EventDto>>();
            _eventRepository = A.Fake<IEventRepository>();
            _sponsorRepository = A.Fake<ISponsorRepository>();
            _speakerRepository = A.Fake<ISpeakerRepository>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<EventService>>();
        }

        [Fact]
        public async Task EventService_GetAllAsync_ReturnsEventDto()
        {
            // Arrange
            var event1 = new Event { Id = 1, Name = "Conference" };
            var event2 = new Event { Id = 2, Name = "Online webinar" };
            var event3 = new Event { Id = 3, Name = "Online community .NET" };
            var events = new List<Event> { event1, event2, event3 };
            var eventDtoList = events.Select(_ => new EventDto { Id = _.Id, Name = _.Name }).ToList();

            A.CallTo(() => _eventRepository.GetAllByAsync(A<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>.Ignored,
                                                          A<Expression<Func<Event, bool>>>.Ignored))
                                                         .Returns(events);

            A.CallTo(() => _mapper.Map<IEnumerable<EventDto>>(events))
                                                             .Returns(eventDtoList);

            var eventService = new EventService(_eventRepository, _sponsorRepository, _speakerRepository, _mapper, _logger);

            // Act
            var result = await eventService.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task EventService_GetByIdAsync_ReturnsEventDto()
        {
            // Arrange
            int eventId = 1;
            var eventEntity = new Event { Id = eventId };
            var eventDto = new EventDto { Id = eventId };

            A.CallTo(() => _eventRepository.GetOneByAsync(A<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>.Ignored,
                                                          A<Expression<Func<Event, bool>>>.Ignored))
                                                         .Returns(eventEntity);

            A.CallTo(() => _mapper.Map<EventDto>(eventEntity))
                                                .Returns(eventDto);

            var eventService = new EventService(_eventRepository, _sponsorRepository, _speakerRepository, _mapper, _logger);

            // Act
            var result = await eventService.GetByIdAsync(eventId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(eventId);
        }

        [Fact]
        public async Task EventService_CreateAsync_ReturnsEventDto()
        {
            // Arrange
            var eventDto = new EventDto
            {
                Id = 1,
                Name = "Test Conference",
                Description = "Test Description",
                Plan = "Test Plan",
                Date = DateTime.Today,
                Location = "Test Location",
            };

            var eventToCreate = A.Fake<Event>();

            A.CallTo(() => _mapper.Map<Event>(eventDto))
                                  .Returns(eventToCreate);

            A.CallTo(() => _sponsorRepository.GetOneManyToManyAsync(A<Func<IQueryable<Sponsor>, IIncludableQueryable<Sponsor, object>>>.Ignored,
                                                                    A<Expression<Func<Sponsor, bool>>>.Ignored))
                                                                   .Returns(new Sponsor());

            A.CallTo(() => _speakerRepository.GetOneManyToManyAsync(A<Func<IQueryable<Speaker>, IIncludableQueryable<Speaker, object>>>.Ignored,
                                                                  A<Expression<Func<Speaker, bool>>>.Ignored))
                                                                 .Returns(new Speaker());

            A.CallTo(() => _eventRepository.CreateAsync(eventToCreate)).Returns(Task.CompletedTask);

            var eventService = new EventService(_eventRepository, _sponsorRepository, _speakerRepository, _mapper, _logger);

            // Act
            var validationResult = await _validator.TestValidateAsync(eventDto);
            var result = await eventService.CreateAsync(eventDto);

            // Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task EventService_UpdateAsync_ReturnsEventDto()
        {
            // Arrange
            var eventId = 1;
            var eventEntityToUpdate = new Event
            {
                Id = 1,
                Name = "Test Conference",
                Description = "Test Description",
                Plan = "Test Plan",
                Date = DateTime.Today,
                Location = "Test Location",
                Sponsors = new List<Sponsor> { A.Fake<Sponsor>(), A.Fake<Sponsor>() },
                Speakers = new List<Speaker> { A.Fake<Speaker>(), A.Fake<Speaker>() },
            };
            var eventToUpdate = new EventDto
            {
                Id = 1,
                Name = "Test Conference",
                Description = "Test Description",
                Plan = "Test Plan",
                Date = DateTime.Today,
                Location = "Test Location", 
                SponsorsIds = new List<int> { 1, 2 },
                SpeakersIds = new List<int> { 1, 3 }
            };

            A.CallTo(() => _eventRepository.GetOneByAsync(A<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>.Ignored,
                                                          A<Expression<Func<Event, bool>>>.Ignored))
                                                         .Returns(eventEntityToUpdate);


            A.CallTo(() => _mapper.Map<EventDto>(eventEntityToUpdate))
                                                .Returns(eventToUpdate);

            var eventService = new EventService(_eventRepository, _sponsorRepository, _speakerRepository, _mapper, _logger);

            // Act
            var validationResult = await _validator.TestValidateAsync(eventToUpdate);
            var result = await eventService.UpdateAsync(eventId, eventToUpdate);

            // Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
            result.Should().NotBeNull();
        }
    }
}