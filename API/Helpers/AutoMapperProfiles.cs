using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterDto, AppUser>();
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl,
                           opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url));
            CreateMap<UpdateMemberDto, AppUser>();
            CreateMap<Photo, PhotoDto>();
            CreateMap<AppUser, FriendDto>()
                .ForMember(dest => dest.PhotoUrl,
                            opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url));
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderUsername,
                            opt => opt.MapFrom(src => src.SenderUser.UserName))
                .ForMember(dest => dest.ReceiverUsername,
                            opt => opt.MapFrom(src => src.ReceiverUser.UserName));
            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.Username,
                            opt => opt.MapFrom(src => src.AppUser.UserName))
                .ForMember(dest => dest.UserPhoto,
                            opt => opt.MapFrom(src => src.AppUser.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.PhotoUrl,
                            opt => opt.MapFrom(src => src.PhotoPost.Url));                            
        }
    }
}