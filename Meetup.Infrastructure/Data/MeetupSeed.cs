namespace Meetup.Infrastructure.Data
{
    public sealed class MeetupSeed
    {
        private readonly ILogger<MeetupSeed> _logger;

        public MeetupSeed(ILogger<MeetupSeed> logger)
        {
            _logger = logger;
        }

        public static async Task SeedAsync(MeetupContext meetupContext, ILogger<MeetupSeed> logger, int retry = 0)
        {
            var retryForAvailability = retry;

            try
            {
                if (!await meetupContext.Events.AnyAsync())
                {
                    await meetupContext.AddRangeAsync(
                        GetPreConfiguredEvents());

                    await meetupContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability >= 10) throw;
                retryForAvailability++;

                logger.LogError(ex.Message);

                await SeedAsync(meetupContext, logger, retryForAvailability);
            }
        }

        private static IEnumerable<Event> GetPreConfiguredEvents()
        {
            var sponsorsForFirstEvent = new List<Sponsor>
            {
                new("Apple"),
                new("Microsoft")                
            };

            var speakersForFirstEvent = new List<Speaker>
            {
                new("John Smith"),
                new("Kate John")
            };

            var sponsorsForSecondEvent = new List<Sponsor>
            {
                new("Google"),
                new("Intel")

            };

            var speakersForSecondEvent = new List<Speaker>
            {
                new("Emily Criss"),
                new("Taylor Grand")
            };

            return new List<Event>
            {
                new("CLEAN ARCHITECTURE MASTERCLASS", "Certification masterclasses for .NET software engineers",
                    ".NET conference Online, practice.",
                    new DateTime(2023, 09, 09, 12, 12, 0),
                    "Online", sponsorsForFirstEvent, speakersForFirstEvent),

                new(".NET DEVELOPER CONFERENCE", " With a variety of talks, workshops, the conference offers developers a wide range of opportunities to learn about important topics in .NET.",
                    "One working day, lunch break and coffee breaks.",
                    new DateTime(2023, 08, 08, 10, 10, 0),
                    "Cologne, Germany. Pullman Cologne Hotel in Cologne.", sponsorsForSecondEvent, speakersForSecondEvent)
            };
        }
    }
}

