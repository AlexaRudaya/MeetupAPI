namespace Meetup.Infrastructure.Repositories
{
    public sealed class SponsorRepository : BaseRepository<Sponsor>, ISponsorRepository
    {
        public SponsorRepository(MeetupContext dbContext) : base(dbContext)
        {
        }
    }
}
