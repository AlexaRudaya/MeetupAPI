namespace Meetup.Infrastructure.Mapper
{
    public sealed class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {
            CreateMap<Sponsor, SponsorDto>().ReverseMap();
            CreateMap<Speaker, SpeakerDto>().ReverseMap();
            CreateMap<Event, EventDto>().ReverseMap();
        }
    }
}
