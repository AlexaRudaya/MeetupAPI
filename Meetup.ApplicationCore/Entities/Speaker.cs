namespace Meetup.ApplicationCore.Entities
{
    public class Speaker : BaseModel
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
