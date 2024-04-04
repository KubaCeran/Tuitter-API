using AutoMapper;
using Core.DTOs.Posts;
using Core.Entities;

namespace Infrastructure.AutomapperProfiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<CreatePostDto, Post>()
                .ForMember(dest => dest.Categories, opt => opt.Ignore()).ReverseMap();

            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.CategoriesNames, opt => opt.MapFrom(src => src.Categories.Select(x => x.Title)))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreationTime));
        }
    }
}
