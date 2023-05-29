namespace Meetup.Infrastructure.Data
{
    public sealed class MeetupContext : DbContext
    {
        public DbSet<Event> Events { get; set; }

        public DbSet<Speaker> Speakers { get; set; }

        public DbSet<Sponsor> Sponsors { get; set; }

        public MeetupContext(DbContextOptions<MeetupContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new EventModelConfiguration());
        }
    }
}
