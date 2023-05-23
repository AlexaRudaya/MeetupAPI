namespace Meetup.Infrastructure.ModelConfiguration
{
    public sealed class EventModelConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder
                .HasMany(_ => _.Sponsors)
                .WithMany();

            builder
               .HasMany(_ => _.Speakers)
               .WithMany();
        }
    }
}
