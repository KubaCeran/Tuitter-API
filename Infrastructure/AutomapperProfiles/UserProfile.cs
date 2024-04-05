using AutoMapper;
using Core.DTOs.Users;
using Core.Entities;

namespace Infrastructure.AutomapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
