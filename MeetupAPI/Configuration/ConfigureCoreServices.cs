namespace MeetupAPI.Configuration
{
    public static class ConfigureCoreServices
    {
        /// <summary>
        /// Applies service installers. Iterating over assemblies using LINQ in order to find types that implement IServiceInstaller interface.
        /// Instantiates them. And call Install() method on the instances of this service installer to register the services.
        /// </summary>
        /// <param name="services">The IServiceCollection to install the services into.</param>
        /// <param name="configuration">The IConfiguration instance for configuring the services.</param>
        /// <param name="logging">For logging configuring.</param>
        /// <param name="assemblies">The assemblies to scan for service installers.</param>
        /// <returns>The modified IServiceCollection.</returns>
        public static IServiceCollection InstallServices(this IServiceCollection services,
               IConfiguration configuration, 
               ILoggingBuilder logging,
               params Assembly[] assemblies)
        {
            var serviceInstallers = assemblies
                       .SelectMany(_ => _.DefinedTypes)  // to take from the assembly all of the defined types
                       .Where(IsAssignableToType<IServiceInstaller>) // filter the types for the ones that implement IServiceInstaller
                       .Select(Activator.CreateInstance) // instantiates service installers
                       .Cast<IServiceInstaller>();

            // Installing all of the services that we have placed inside service installers
            foreach (var serviceInstaller in serviceInstallers)
            {
                serviceInstaller.Install(services, configuration, logging);
            }

            return services;

            // Checks if the given type is assignable to a generic type that is specified in an argument
            static bool IsAssignableToType<T>(TypeInfo typeInfo) =>
                   typeof(T).IsAssignableFrom(typeInfo) &&
                   !typeInfo.IsInterface &&
                   !typeInfo.IsAbstract;
        }
    }
}