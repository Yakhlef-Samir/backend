using AutoMapper;
using WeCount.Application.Common.Mapping.MappingProfiles;

namespace WeCount.Application.Common.Mapping.UserMapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            new UserMappingProfile();
        }
    }
}
