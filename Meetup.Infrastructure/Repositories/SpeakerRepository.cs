namespace Meetup.Infrastructure.Repositories
{
    public sealed class SpeakerRepository : BaseRepository<Speaker>, ISpeakerRepository
    {
        public SpeakerRepository(MeetupContext dbContext) : base(dbContext)
        {
        }
    }
}
