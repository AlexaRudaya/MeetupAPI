namespace MeetupAPI.Interfaces
{
    /// <summary>
    /// Defines Service installers that are needed in Application to register services.
    /// </summary>
    public interface IServiceInstaller
    {
        void Install(IServiceCollection services, IConfiguration configuration, ILoggingBuilder logging);
    }
}