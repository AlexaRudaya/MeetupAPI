namespace Meetup.ApplicationCore.Entities
{
    public sealed class Speaker : BaseModel
    {
        [Required]
        public string? Name { get; set; }

        public Speaker()
        {               
        }

        public Speaker(string name)
        {
            Name = name;  
        }
    }
}
