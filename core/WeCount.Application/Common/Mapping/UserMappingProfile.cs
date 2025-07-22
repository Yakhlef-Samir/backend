using AutoMapper;
using WeCount.Application.DTOs;
using WeCount.Domain.Entities;

namespace WeCount.Application.Common.Mapping.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.Name.FirstName} {src.Name.LastName}")
                );
        }
    }
}
