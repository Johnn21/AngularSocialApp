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
        }
    }
}