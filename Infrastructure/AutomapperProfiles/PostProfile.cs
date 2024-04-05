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
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.CategoriesNames, opt => opt.MapFrom(src => src.Categories.Select(x => x.Title)))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreationTime))
                .ForMember(dest => dest.NextLevelRepliesCount, opt => opt.MapFrom(src => src.Replies.Count(x => x.ParentPostId == src.Id)));
        }
    }
}
