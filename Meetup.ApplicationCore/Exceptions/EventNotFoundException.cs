namespace Meetup.ApplicationCore.Exceptions
{
    public sealed class EventNotFoundException : Exception
    {
        public EventNotFoundException(string message) : base(message)
        {                
        }
    }
}
