namespace MeetupAPI.Configuration
{
    public sealed class AuthenticationAndAuthorizationServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration, ILoggingBuilder logging)
        {
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

            #region JWT

            services.AddSwaggerGen(_ =>
            {
                _.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. " +
                    "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                _.AddSecurityRequirement(new OpenApiSecurityRequirement
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
        }
    }
}
