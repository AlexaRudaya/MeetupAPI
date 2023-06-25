namespace MeetupAPI.Configuration
{
    public sealed class WebServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration, ILoggingBuilder logging)
        {
            #region Logger

            logging.ClearProviders();
            logging.AddSerilog(
                new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger());

            #endregion

            #region Middlewares

            services.AddTransient<GlobalExceptionHandlingMiddleware>();

            #endregion
        }
    }
}
