namespace Meetup.ApplicationCore.Entities
{
    public sealed class Event : BaseModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Plan { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? Date { get; set; }

        [Required]
        public string? Location { get; set;}

        public List<Sponsor>? Sponsors { get; set; } = new();

        public List<Speaker>? Speakers { get; set; } = new();

        public Event()
        {
        }

        public Event(string name, string description, string plan,
            DateTime date, string location, List<Sponsor> sponsors,
            List<Speaker> speakers)
        {
            Name = name;
            Description = description;
            Plan = plan;
            Date = date;
            Location = location;
            Sponsors = sponsors;
            Speakers = speakers;
        }
    }
}
