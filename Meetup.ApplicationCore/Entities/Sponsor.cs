namespace Meetup.ApplicationCore.Entities
{
    public sealed class Sponsor : BaseModel
    {
        [Required]
        public string? Name { get; set; }

        public Sponsor() 
        {
        }

        public Sponsor(string name)
        {
            Name = name;
        }
    }
}
