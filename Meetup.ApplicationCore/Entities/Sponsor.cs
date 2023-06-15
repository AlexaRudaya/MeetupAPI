namespace Meetup.ApplicationCore.Entities
{
    public class Sponsor : BaseModel
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
