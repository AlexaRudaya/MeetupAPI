namespace Meetup.ApplicationCore.DTO
{
    public sealed class EventDto : BaseDto
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Plan { get; set; }

        public DateTime? Date { get; set; }

        public string? Location { get; set; }

        public List<int> SponsorsIds { get; set; } = new();

        public List<Sponsor>? Sponsors { get; set; } = new();

        public List<int> SpeakersIds { get; set; } = new();

        public List<Speaker>? Speakers { get; set; } = new();
    }
}
