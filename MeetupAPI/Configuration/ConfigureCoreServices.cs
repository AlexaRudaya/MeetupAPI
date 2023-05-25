namespace MeetupAPI.Configuration
{
    public static class ConfigureCoreServices
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services,
            ILoggingBuilder logging)
        {
            #region Logger

            logging.ClearProviders();
            logging.AddSerilog(
                new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger());

            #endregion

            #region DB

            services.AddDbContext<MeetupContext>(_ => _
               .UseSqlServer(configuration.GetConnectionString("MeetupConnection"))
               .EnableSensitiveDataLogging());

            #endregion

            #region Validation

            services.AddScoped<IValidator<SponsorDto>,SponsorValidation>();
            services.AddScoped<IValidator<SpeakerDto>, SpeakerValidation>();
            services.AddScoped<IValidator<EventDto>, EventValidation>();

            #endregion

            #region Services

            services.AddScoped<ISponsorRepository, SponsorRepository>();
            services.AddAutoMapper(typeof(MapperProfile));

            #endregion
        }

    }
}
