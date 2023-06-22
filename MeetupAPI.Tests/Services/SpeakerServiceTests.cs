using FakeItEasy;
using FluentValidation.TestHelper;

namespace MeetupAPI.Tests.Services
{
    public class SpeakerServiceTests
    {
        private readonly IValidator<SpeakerDto> _validator;
        private readonly IMapper _mapper;
        private readonly ISpeakerRepository _speakerRepository;
        private readonly ILogger<SpeakerService> _logger;

        public SpeakerServiceTests()
        {
            _validator = A.Fake<IValidator<SpeakerDto>>();
            _mapper = A.Fake<IMapper>();
            _speakerRepository = A.Fake<ISpeakerRepository>();
            _logger = A.Fake<ILogger<SpeakerService>>();
        }

        [Fact]
        public async Task SpeakerService_GetAllAsync_ReturnsSpeakersDto()
        {
            // Arrange
            var speaker1 = new Speaker { Id = 1, Name = "Lilly Mi" };
            var speaker2 = new Speaker { Id = 2, Name = "Mike Stape" };
            var speaker3 = new Speaker { Id = 3, Name = "Nike Lil" };
            var speakers = new List<Speaker> { speaker1, speaker2, speaker3 };
            var speakerDtoList = speakers.Select(_ => new SpeakerDto { Id = _.Id, Name = _.Name })
                                         .ToList();

            A.CallTo(() => _speakerRepository.GetAllByAsync(A<Func<IQueryable<Speaker>, IIncludableQueryable<Speaker, object>>>.Ignored,
                                                            A<Expression<Func<Speaker, bool>>>.Ignored))
                                                           .Returns(speakers);

            A.CallTo(() => _mapper.Map<IEnumerable<SpeakerDto>>(speakers))
                                                                .Returns(speakerDtoList);

            var speakerService = new SpeakerService(_speakerRepository, _mapper, _logger);

            // Act
            var result = await speakerService.GetAllAsync();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(3)
                 .And.OnlyHaveUniqueItems();
            result.Should().BeInAscendingOrder(_ => _.Name);
        }

        [Fact]
        public async Task SpeakerService_GetByIdAsync_ReturnsSpeakerDto()
        {
            // Arrange
            var speakerId = 1;
            var speakerEntity = new Speaker { Id = speakerId };
            var speakerDto = new SpeakerDto { Id = speakerId };

            A.CallTo(() => _speakerRepository.GetOneByAsync(A<Func<IQueryable<Speaker>, IIncludableQueryable<Speaker, object>>>.Ignored,
                                                            A<Expression<Func<Speaker, bool>>>.Ignored))
                                                           .Returns(speakerEntity);

            A.CallTo(() => _mapper.Map<SpeakerDto>(speakerEntity))
                                                  .Returns(speakerDto);

            var speakerService = new SpeakerService(_speakerRepository, _mapper, _logger);

            // Act
            var result = await speakerService.GetByIdAsync(speakerId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(speakerId);
            speakerId.Should().Be(1);
        }

        [Fact]
        public async Task SpeakerService_CreateAsync_ReturnsSpeakerDto()
        {
            // Arrange
            var speakerDto = new SpeakerDto
            {
                Id = 1,
                Name = "Jimmy Spark"
            };

            var speakerToCreate = A.Fake<Speaker>();

            A.CallTo(() => _mapper.Map<Speaker>(speakerDto))
                                               .Returns(speakerToCreate);

            A.CallTo(() => _speakerRepository.CreateAsync(speakerToCreate))
                                             .Returns(Task.CompletedTask);

            var speakerService = new SpeakerService(_speakerRepository, _mapper, _logger);

            // Act
            var validationResult = await _validator.TestValidateAsync(speakerDto);
            var result = await speakerService.CreateAsync(speakerDto);

            // Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task SpeakerService_UpdateAsync_ReturnsSpeakerDto()
        {
            // Arrange
            var speakerId = 1;
            var speakerEntityToUpdate = new Speaker
            {
                Id = 1,
                Name = "Lilly Mi"
            };
            var speakerToUpdate = new SpeakerDto
            {
                Id = 1,
                Name = "Lilly Milly"
            };

            A.CallTo(() => _speakerRepository.GetOneByAsync(A<Func<IQueryable<Speaker>, IIncludableQueryable<Speaker, object>>>.Ignored,
                                                            A<Expression<Func<Speaker, bool>>>.Ignored))
                                                           .Returns(speakerEntityToUpdate);

            A.CallTo(() => _mapper.Map<Speaker>(speakerToUpdate))
                                                  .Returns(speakerEntityToUpdate);

            A.CallTo(() => _speakerRepository.UpdateAsync(speakerEntityToUpdate))
                                             .Returns(Task.CompletedTask);

            var speakerService = new SpeakerService(_speakerRepository, _mapper, _logger);

            // Act
            var validationResult = await _validator.TestValidateAsync(speakerToUpdate);
            var result = await speakerService.UpdateAsync(speakerId, speakerToUpdate);

            // Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
            result.Should().NotBeNull();
            speakerId.Should().Be(1);
        }

        [Fact]
        public async Task SpeakerService_DeleteAsync_ReturnsSpeakerDto()
        {
            // Arrange
            int speakerId = 4;
            var speakerEntityToDelete = new Speaker
            {
                Id = speakerId
            };
            var speakerToDelete = new SpeakerDto
            {
                Id = speakerId
            };

            A.CallTo(() => _speakerRepository.GetOneByAsync(A<Func<IQueryable<Speaker>, IIncludableQueryable<Speaker, object>>>.Ignored,
                                                            A<Expression<Func<Speaker, bool>>>.Ignored))
                                                           .Returns(speakerEntityToDelete);

            A.CallTo(() => _mapper.Map<SpeakerDto>(speakerEntityToDelete))
                                                  .Returns(speakerToDelete);

            A.CallTo(() => _speakerRepository.DeleteAsync(speakerEntityToDelete))
                                             .Returns(Task.CompletedTask);

            var speakerService = new SpeakerService(_speakerRepository, _mapper, _logger);

            // Act
            var result = await speakerService.DeleteAsync(speakerId);

            // Assert
            result.Should().BeEquivalentTo(speakerEntityToDelete);
            speakerId.Should().Be(4);
        }
    }
}