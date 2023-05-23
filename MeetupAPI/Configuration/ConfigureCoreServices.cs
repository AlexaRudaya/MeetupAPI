namespace MeetupAPI.Configuration
{
    public static class ConfigureCoreServices
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services,
            ILoggingBuilder logging)
        {
            services.AddDbContext<MeetupContext>(_ => _.UseSqlServer(configuration.GetConnectionString("MeetupConnection")));
        }
    }
}
