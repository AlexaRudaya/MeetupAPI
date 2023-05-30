namespace Meetup.ApplicationCore.Interfaces.IRepository
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        public Task UpdateEventAsync(Event eventModel, List<int> sponsorsIds, List<int> speakersIds);
    }
}
