namespace MeetupAPI.Configuration
{
    public sealed class ApplicationServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration, ILoggingBuilder logging)
        {

            #region Validation

            services.AddScoped<IValidator<SponsorDto>, SponsorValidation>();
            services.AddScoped<IValidator<SpeakerDto>, SpeakerValidation>();
            services.AddScoped<IValidator<EventDto>, EventValidation>();

            #endregion

            #region AutoMapper

            services.AddAutoMapper(typeof(MapperProfile));

            #endregion

            #region Core Services

            services.AddScoped<ISponsorService, SponsorService>();
            services.AddScoped<ISpeakerService, SpeakerService>();
            services.AddScoped<IEventService, EventService>();

            #endregion
        }
    }
}
