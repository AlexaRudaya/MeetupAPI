namespace MeetupAPI.Tests.Services
{
    public class SponsorServiceTests
    {
        private readonly IValidator<SponsorDto> _validator;
        private readonly IMapper _mapper;
        private readonly ISponsorRepository _sponsorRepository; 
        private readonly ILogger<SponsorService> _logger;

        public SponsorServiceTests()
        {
            _validator = A.Fake<IValidator<SponsorDto>>();
            _mapper = A.Fake<IMapper>();
            _sponsorRepository = A.Fake<ISponsorRepository>();
            _logger = A.Fake<ILogger<SponsorService>>();
        }

        [Fact]
        public async Task SponsorService_GetAllAsync_ReturnsSponsorsDto()
        {
            // Arrange
            var sponsor1 = new Sponsor { Id = 1, Name = "Lenovo" };
            var sponsor2 = new Sponsor { Id = 2, Name = "Microsoft" };
            var sponsor3 = new Sponsor { Id = 3, Name = "Google" };
            var sponsors = new List<Sponsor> { sponsor1, sponsor2, sponsor3 };
            var sponsorDtoList = sponsors.Select(_ => new SponsorDto { Id = _.Id, Name = _.Name })
                                         .ToList();

            A.CallTo(() => _sponsorRepository.GetAllByAsync(A<Func<IQueryable<Sponsor>, IIncludableQueryable<Sponsor, object>>>.Ignored,
                                                            A<Expression<Func<Sponsor, bool>>>.Ignored))
                                                           .Returns(sponsors);

            A.CallTo(() => _mapper.Map<IEnumerable<SponsorDto>>(sponsors))
                                                                .Returns(sponsorDtoList);

            var sponsorService = new SponsorService(_sponsorRepository, _mapper, _logger);

            // Act
            var result = await sponsorService.GetAllAsync();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(3)
                 .And.OnlyHaveUniqueItems();
            result.Should().BeInAscendingOrder(_ => _.Name);
        }

        [Fact]
        public async Task SponsorService_GetByIdAsync_ReturnsSponsorDto()
        {
            // Arrange
            var sponsorId = 1;
            var sponsorEntity = new Sponsor { Id = sponsorId};
            var sponsorDto = new SponsorDto { Id = sponsorId};

            A.CallTo(() => _sponsorRepository.GetOneByAsync(A<Func<IQueryable<Sponsor>, IIncludableQueryable<Sponsor, object>>>.Ignored,
                                                            A<Expression<Func<Sponsor, bool>>>.Ignored))
                                                           .Returns(sponsorEntity);

            A.CallTo(() => _mapper.Map<SponsorDto>(sponsorEntity))
                                                  .Returns(sponsorDto);

            var sponsorService = new SponsorService(_sponsorRepository, _mapper, _logger);

            // Act
            var result = await sponsorService.GetByIdAsync(sponsorId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(sponsorId);
            sponsorId.Should().Be(1);
        }

        [Fact]
        public async Task SponsorService_CreateAsync_ReturnsSponsorDto()
        {
            // Arrange
            var sponsorDto = new SponsorDto
            {
                Id = 1,
                Name = "Apple"
            };

            var sponsorToCreate = A.Fake<Sponsor>();

            A.CallTo(() => _mapper.Map<Sponsor>(sponsorDto))
                                               .Returns(sponsorToCreate);

            A.CallTo(() => _sponsorRepository.CreateAsync(sponsorToCreate))
                                             .Returns(Task.CompletedTask);

            var sponsorService = new SponsorService(_sponsorRepository, _mapper, _logger);

            // Act
            var validationResult = await _validator.TestValidateAsync(sponsorDto);
            var result = await sponsorService.CreateAsync(sponsorDto);

            // Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task SponsorService_UpdateAsync_ReturnsSponsorDto()
        {
            // Arrange
            var sponsorId = 1;
            var sponsorEntityToUpdate = new Sponsor
            {
                Id = 1,
                Name = "Apple"
            };
            var sponsorToUpdate = new SponsorDto
            {
                Id = 1,
                Name = "Oracle"
            };

            A.CallTo(() => _sponsorRepository.GetOneByAsync(A<Func<IQueryable<Sponsor>, IIncludableQueryable<Sponsor, object>>>.Ignored,
                                                            A<Expression<Func<Sponsor, bool>>>.Ignored))
                                                           .Returns(sponsorEntityToUpdate);

            A.CallTo(() => _mapper.Map<Sponsor>(sponsorToUpdate))
                                                  .Returns(sponsorEntityToUpdate);

            A.CallTo(() => _sponsorRepository.UpdateAsync(sponsorEntityToUpdate))
                                             .Returns(Task.CompletedTask);

            var sponsorService = new SponsorService(_sponsorRepository, _mapper, _logger);

            // Act
            var validationResult = await _validator.TestValidateAsync(sponsorToUpdate);
            var result = await sponsorService.UpdateAsync(sponsorId, sponsorToUpdate);

            // Assert
            validationResult.ShouldNotHaveAnyValidationErrors();
            result.Should().NotBeNull();
            sponsorId.Should().Be(1);
        }

        [Fact]
        public async Task SponsorService_DeleteAsync_ReturnsSponsorDto()
        {
            // Arrange
            int sponsorId = 4;
            var sponsorEntityToDelete = new Sponsor
            {
                Id = sponsorId
            };
            var sponsorToDelete = new SponsorDto
            {
                Id = sponsorId
            };

            A.CallTo(() => _sponsorRepository.GetOneByAsync(A<Func<IQueryable<Sponsor>, IIncludableQueryable<Sponsor, object>>>.Ignored,
                                                            A<Expression<Func<Sponsor, bool>>>.Ignored))
                                                           .Returns(sponsorEntityToDelete);

            A.CallTo(() => _mapper.Map<SponsorDto>(sponsorEntityToDelete))
                                                  .Returns(sponsorToDelete);

            A.CallTo(() => _sponsorRepository.DeleteAsync(sponsorEntityToDelete))
                                             .Returns(Task.CompletedTask);

            var sponsorService = new SponsorService(_sponsorRepository, _mapper, _logger);

            // Act
            var result = await sponsorService.DeleteAsync(sponsorId);

            // Assert
            result.Should().BeEquivalentTo(sponsorEntityToDelete);
            sponsorId.Should().Be(4);
        }
    }
}