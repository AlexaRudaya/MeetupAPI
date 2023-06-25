namespace MeetupAPI.Configuration
{
    public sealed class InfrastructureServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration, ILoggingBuilder logging)
        {
            #region Database

            services.AddDbContext<MeetupContext>(_ => _
               .UseSqlServer(configuration.GetConnectionString("MeetupConnection")));

            #endregion

            #region Repositories

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ISponsorRepository, SponsorRepository>();
            services.AddScoped<ISpeakerRepository, SpeakerRepository>();
            services.AddScoped<IEventRepository, EventRepository>();

            #endregion

            #region RabbitMQ Services

            services.AddScoped<IProducerService, ProducerService>();

            #endregion
        }
    }
}
