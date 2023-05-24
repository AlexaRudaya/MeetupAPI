namespace Meetup.Infrastructure.Repositories
{
    public sealed class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(MeetupContext dbContext) : base(dbContext)
        {
        }
    }
}
