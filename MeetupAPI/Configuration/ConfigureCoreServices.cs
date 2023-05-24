using Meetup.ApplicationCore.Interfaces.IRepository;
using Meetup.Infrastructure.Repositories;

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

            #region Services

            services.AddScoped<ISponsorRepository, SponsorRepository>();
            services.AddAutoMapper(typeof(MapperProfile));

            #endregion
        }

    }
}
