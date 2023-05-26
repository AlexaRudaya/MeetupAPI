namespace Meetup.ApplicationCore.DTO
{
    public abstract class BaseDto
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
    }
}
