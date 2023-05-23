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
                if (!await meetupContext.Sponsors.AnyAsync())
                {
                    await meetupContext.AddRangeAsync(
                        GetPreConfiguredSponsors());

                    await meetupContext.SaveChangesAsync();
                }

                if (!await meetupContext.Speakers.AnyAsync())
                {
                    await meetupContext.AddRangeAsync(
                        GetPreConfiguredSpeakers());

                    await meetupContext.SaveChangesAsync();
                }

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
            var sponsors = GetPreConfiguredSponsors().ToList();
            var speakers = GetPreConfiguredSpeakers().ToList();

            var sponsorsForFirstEvent = new List<Sponsor>
            {
                sponsors.FirstOrDefault(_ => _.Name == "Google")!,
                sponsors.FirstOrDefault(_ => _.Name == "Microsoft")! 
            };

            var speakersForFirstEvent = new List<Speaker>
            {
                speakers.FirstOrDefault(_ => _.Name == "Kate John")!,
                speakers.FirstOrDefault(_ => _.Name == "Emily Criss")!
            };

            var sponsorsForSecondEvent = new List<Sponsor>
            {
                sponsors.FirstOrDefault(_ => _.Name == "Microsoft")!,
                sponsors.FirstOrDefault(_ => _.Name == "Apple")! 
            };

            var speakersForSecondEvent = new List<Speaker>
            {
                speakers.FirstOrDefault(_ => _.Name == "John Smith")!,
                speakers.FirstOrDefault(_ => _.Name == "Taylor Grand")! 
            };

            return new List<Event>
            {
                new("CLEAN ARCHITECTURE MASTERCLASS", "Certification masterclasses for .NET software engineers",
                    ".NET conference Online, practice.",
                    new DateTime(2023, 09, 09, 12, 12, 0),
                    "Online", sponsorsForFirstEvent, speakersForFirstEvent),

                new(".NET DEVELOPER CONFERENCE", " With a variety of talks. workshops, the conference offers developers a wide range of opportunities to learn about important topics in .NET.",
                    "One working day, lunch break and coffee breaks.",
                    new DateTime(2023, 08, 08, 10, 10, 0),
                    "Cologne, Germany. Pullman Cologne Hotel in Cologne.", sponsorsForSecondEvent, speakersForSecondEvent)
            };
        }

        private static IEnumerable<Sponsor> GetPreConfiguredSponsors()
        {
            return new List<Sponsor>
            {
                new("Apple"),
                new("Microsoft"),
                new("Google")
            };
        }

        private static IEnumerable<Speaker> GetPreConfiguredSpeakers()
        {
            return new List<Speaker>
            {
                new("John Smith"),
                new("Kate John"),
                new("Emily Criss"),
                new("Taylor Grand")
            };
        }
    }
}

