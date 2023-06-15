namespace Meetup.Infrastructure.Repositories
{
    public sealed class EventRepository : BaseRepository<Event>, IEventRepository
    {
        private readonly MeetupContext _dbContext;

        public EventRepository(MeetupContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Updates an event.
        /// </summary>
        /// <param name="eventModel">The event to be updated.</param>
        /// <param name="sponsorsIds">A list of sponsors IDs associated with the event.</param>
        /// <param name="speakersIds">A list of speakers IDs associated with the event.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateEventAsync(Event eventModel, List<int> sponsorsIds, List<int> speakersIds)
        {
            var model = await _dbContext.Events.Include(_ => _.Sponsors)
                                               .Include(_ => _.Speakers)
                                               .AsSplitQuery()
                                               .FirstOrDefaultAsync(_ => _.Id.Equals(eventModel.Id));

            _dbContext.Entry(model!)
                             .CurrentValues.SetValues(eventModel); 

            model.Sponsors!.Clear(); 
            model.Speakers!.Clear();

            foreach (var item in sponsorsIds)
            {
                var sponsorFromDb = await _dbContext.Sponsors.FirstOrDefaultAsync(_ => _.Id.Equals(item));
                
                if (sponsorFromDb is null) 
                {
                    _dbContext.Attach(sponsorFromDb!); 
                }
                else 
                {
                    model.Sponsors.Add(sponsorFromDb);
                }
            }

            foreach (var item in speakersIds)
            {
                var speakerFromDb = await _dbContext.Speakers.FirstOrDefaultAsync(_ => _.Id.Equals(item));

                if (speakerFromDb is null)
                {
                    _dbContext.Attach(speakerFromDb!);
                }
                else
                {
                    model.Speakers.Add(speakerFromDb);
                }
            }

           _dbContext.Entry(model).State = EntityState.Modified; 

            await _dbContext.SaveChangesAsync();
        }
    }
}
