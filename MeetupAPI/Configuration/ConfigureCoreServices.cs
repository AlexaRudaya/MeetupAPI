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

            services.AddScoped<IValidator<SponsorDto>, SponsorValidation>();
            services.AddScoped<IValidator<SpeakerDto>, SpeakerValidation>();
            services.AddScoped<IValidator<EventDto>, EventValidation>();

            #endregion

            #region Services

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ISponsorRepository, SponsorRepository>();
            services.AddScoped<ISpeakerRepository, SpeakerRepository>();
            services.AddScoped<IEventRepository, EventRepository>();

            services.AddScoped<ISponsorService, SponsorService>();
            services.AddScoped<ISpeakerService, SpeakerService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IProducerService, ProducerService>();

            services.AddAutoMapper(typeof(MapperProfile));

            #endregion

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

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. " +
                    "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            #endregion

            #region IdentityServer

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    // Define here URL on which the IdentityServer is up and running
                    options.Authority = configuration["IdentityServer:Authority"];

                    // Define here the name of the Resource (API)
                    options.ApiName = configuration["IdentityServer:ApiName"];

                    options.RequireHttpsMetadata = false; 
                });

            #endregion
        }
    }
}
