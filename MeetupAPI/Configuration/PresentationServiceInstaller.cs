namespace MeetupAPI.Configuration
{
    public sealed class PresentationServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration, ILoggingBuilder logging)
        {
            #region Swagger

            services.AddSwaggerGen(_ =>
            {
                _.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Meetup API",
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                _.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                _.EnableAnnotations();
            });

            #endregion

            services.AddControllers();
            services.AddEndpointsApiExplorer();
        }
    }
}
