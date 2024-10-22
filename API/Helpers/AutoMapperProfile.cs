namespace API.Helpers;

using API.DTOs;
using API.Entities;
using AutoMapper;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberReponse>()
        .ForMember(
            dest => dest.PhotoUrl,
            ori => ori.MapFrom(
                source => source.Photos.FirstOrDefault(p => p.IsMain)!.Url
            )
        );
        CreateMap<Photo, PhotoResponse>();

    }
}