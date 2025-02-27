namespace API.Helpers;

using System.Globalization;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Localization;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberReponse>()
        .ForMember(
            d => d.Age,
            o => o.MapFrom(s => s.BirthDay.CalculateAge())
        )
        .ForMember(
            dest => dest.PhotoUrl,
            ori => ori.MapFrom(
                source => source.Photos.FirstOrDefault(p => p.IsMain)!.Url
            )
        );
        CreateMap<Photo, PhotoResponse>();
        CreateMap<MemberUpdateRequest, AppUser>();
        CreateMap<RegisterRequest, AppUser>();
        CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s, CultureInfo.InvariantCulture));
    }
}